using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Services;
public class UpdateHandler : IUpdateHandler
{
    private readonly TelegramBotService _tlgramService;
    private readonly ILogger<UpdateHandler> _logger;

    public UpdateHandler(
        TelegramBotService tlgramService,
        ILogger<UpdateHandler> logger)
    {
        _tlgramService = tlgramService;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message),
            { EditedMessage: { } message } => OnMessage(message),
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
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

        await (messageText.Split(' ')[0].ToLower() switch
        {
            "/ask" => _tlgramService.SendAskGoogleAI(msg),
            "/note" => _tlgramService.HandleNoteCommand(msg),
            "/help" => _tlgramService.UsageCommon(msg),
            "/miniapp" => _tlgramService.SendMiniAppLauncher(msg),
            "/brand" => _tlgramService.UsageCommon(msg),
        
            _ => _tlgramService.UsageCommon(msg)
        });

        return;
    }
}