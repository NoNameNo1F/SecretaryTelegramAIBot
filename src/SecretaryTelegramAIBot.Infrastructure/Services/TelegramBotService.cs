using MediatR;
using SecretaryTelegramAIBot.Application.Commands;
using SecretaryTelegramAIBot.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SecretaryTelegramAIBot.Infrastructure.Services;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMediator _mediator;

    public TelegramBotService(ITelegramBotClient botClient, IMediator mediator)
    {
        _botClient = botClient;
        _mediator = mediator;
    }

    public async Task<Message> HandleNoteCommand(Message message)
    {
        var parts = message.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return await UsageCommon(message);

        return await (parts[1].ToLower() switch
        {
            "add" => HandleAddNote(message),
            "export" => HandleExportNote(message),

            _ => UsageCommon(message)
        });
    }

    public async Task<Message> UsageCommon(Message message)
    {
        string msg = """
            *Secretary AI Bot \-\- Help*

            A bot for managing your notes and querying Google AI\.

            *Usage*
            `command [options] [arguments]`

            *Commands*
            • `/help`
            _Show this help message_
            • `/note add <brand> <content>`
            _Add a new note with a brand and content_
            • `/note export [text|excel] [today|weekly]`
            _Export your notes as a file_
            • `/ask <question>`
            _Ask any question to Google AI_

            *Arguments & Placeholders*
            • `<brand>`: The brand name for the note \(e\.g\., `Nike`\)
            • `<content>`: The full content of the note \(e\.g\., `Sent the new deal\!`\)
            • `<question>`: The question to ask Google AI \(e\.g\., `What is the capital of France\?`\)

            *Examples*
            ` /note add "Apple Inc" Finalized the Q3 report\.`
            ` /note export excel weekly`
            ` /ask What are the main principles of DDD\?`
            """;

        return await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: msg,
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: new ReplyKeyboardRemove());
    }

    public async Task<Message> SendAskGoogleAI(Message message)
    {
        var parts = message.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return await UsageCommon(message);

        var question = string.Join(" ", parts[1..]);
        var response = await _mediator.Send(new AskCommand(question), CancellationToken.None);

        return await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: response,
            cancellationToken: CancellationToken.None);
    }

    public async Task<Message> HandleAddNote(Message message)
    {
        var parts = message.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
            return await UsageCommon(message);

        var brandName = parts[2];
        var content = string.Join(" ", parts[3..]);

        await _mediator.Send(new AddNoteCommand(message.Chat.Id, content, brandName), CancellationToken.None);
        return await _botClient.SendMessage(
            chatId: message.Chat.Id,
            text: $"Noted Brand {brandName}: {content}",
            cancellationToken: CancellationToken.None);
    }

    public async Task<Message> HandleExportNote(Message message)
    {
        var parts = message.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
            return await UsageCommon(message);

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
                chatId: message.Chat.Id,
                document: new InputFileStream(fileStream, fileName),
                caption: "Exported notes as text",
                cancellationToken: CancellationToken.None);
        }
        else
        {
            await File.WriteAllBytesAsync(filePath, (byte[])result);
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await _botClient.SendDocument(
                chatId: message.Chat.Id,
                document: new InputFileStream(fileStream, fileName),
                caption: "Exported notes as Excel",
                cancellationToken: CancellationToken.None);
        }
    }
}