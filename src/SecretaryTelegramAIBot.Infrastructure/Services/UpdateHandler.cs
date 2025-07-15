using MediatR;
using Microsoft.Extensions.Logging;
using SecretaryTelegramAIBot.Application.Commands;
using SecretaryTelegramAIBot.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SecretaryTelegramAIBot.Infrastructure.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateHandler> _logger;
    
    public UpdateHandler(ITelegramBotClient botClient, IMediator mediator, ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message),
            { EditedMessage: { } message }                  => OnMessage(message),
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("HandleError: {Exception}", exception);
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
    
    private async Task OnMessage(Message msg)
    {
        _logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
            return;

        Message sentMessage = await (messageText.Split(' ')[0].ToLower() switch
        {
            "/ask" => SendAskGoogleAI(msg),
            "/note" => HandleNoteCommand(msg),
            "/help" => Usage(msg),
            
            _ => Usage(msg)
        });
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> HandleNoteCommand(Message msg)
    {
        if (msg.Text is not { } messageText)
            return await Usage(msg);
        
        var parts = messageText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return await Usage(msg);
        
        return await (parts[1].ToLower() switch
        {
            "add" => HandleAddNote(msg),
            "export" => HandleExportNote(msg),
            
            _ => Usage(msg)
        });
    }
    
    async Task<Message> Usage(Message msg)
    {
        const string usage = """
                        Telegram Bot --help

                        A bot for managing notes and querying Google AI

                        Usage: [COMMAND]

                        Commands:
                          /help                           Show this help message
                          /note add <brand> <content>     Add a new note with a brand name and content
                          /note export text today         Export today's notes as plain text
                          /note export text weekly        Export this week's notes as plain text
                          /note export excel today        Export today's notes as an Excel file
                          /note export excel weekly       Export this week's notes as an Excel file
                          /ask <question>                 Ask a question to Google AI

                        Arguments:
                          <brand>                         The brand name for the note (e.g., Nike)
                          <content>                       The content of the note (e.g., Great shoes!)
                          <question>                      The question to ask Google AI

                        Options:
                          -h, --help                      Print this help message (same as /help)

                        Examples:
                          /note add Nike Sent the deal!   Adds a note for Nike
                          /note export excel today        Exports today's notes in Excel format
                          /ask What's the weather?        Queries Google AI about the weather
                        """;
        
        return await _botClient.SendMessage(msg.Chat, usage, parseMode: ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> SendAskGoogleAI(Message msg)
    {
        // await _botClient.SendChatAction(msg.Chat, ChatAction.UploadPhoto);
        // await Task.Delay(2000);
        // await using var fileStream = new FileStream("Files/bot.gif", FileMode.Open, FileAccess.Read);
        // return await _botClient.SendPhoto(msg.Chat, fileStream, caption: "Read https://telegrambots.github.io/book/");
        
        if (msg.Text is not { } messageText)
            return await Usage(msg);

        var parts = messageText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return await Usage(msg);

        var question = string.Join(" ", parts[1..]);
        return await _botClient.SendMessage(
            chatId: msg.Chat.Id,
            text: $"you asked: {question}",
            cancellationToken: CancellationToken.None);
    }
    
    async Task<Message> HandleAddNote(Message msg)
    {
        if (msg.Text is not { } messageText)
            return await Usage(msg);

        var parts = messageText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
            return await Usage(msg);

        var brandName = parts[2];
        var content = string.Join(" ", parts[3..]);

        await _mediator.Send(new AddNoteCommand(msg.Chat.Id, content, brandName), CancellationToken.None);
        return await _botClient.SendMessage(
            chatId: msg.Chat.Id,
            text: $"Noted Brand {brandName}: {content}",
            cancellationToken: CancellationToken.None);
    }
    
    async Task<Message> HandleExportNote(Message msg)
    {
        if (msg.Text is not { } messageText)
            return await Usage(msg);
        
        var parts = messageText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
            return await Usage(msg);

        var format = parts[2].ToLower();
        var period = parts[3].ToLower();

        var range = period switch
        {
            "today" => ERange.DAILY,
            "weekly" => ERange.WEEKLY,
            _ => ERange.DAILY
        };

        var exportType = format switch
        {
            "text" => ExportType.TEXT,
            "excel" => ExportType.EXCEL,
            _ => ExportType.TEXT
        };
        
        var fileName = $"notes_{DateTime.UtcNow:yyyyMMddHHmmss}.{format}";
        var filePath = Path.Combine(Path.GetTempPath(), fileName);
        
        var result = await _mediator.Send(new ExportNoteCommand(range, exportType), CancellationToken.None);
        if (exportType == ExportType.TEXT)
        {
            await File.WriteAllTextAsync(filePath, result.ToString());
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await _botClient.SendDocument(
                chatId: msg.Chat.Id,
                document: new InputFileStream(fileStream, fileName),
                caption: "Exported notes as text",
                cancellationToken: CancellationToken.None);
        }
        else
        {
            await File.WriteAllBytesAsync(filePath, (byte[])result);
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await _botClient.SendDocument(
                chatId: msg.Chat.Id,
                document: new InputFileStream(fileStream, fileName),
                caption: "Exported notes as Excel",
                cancellationToken: CancellationToken.None);
        }
    }
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}