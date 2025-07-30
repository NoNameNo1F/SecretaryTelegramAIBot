using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Abstractions;
public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService where TUpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly TUpdateHandler _updateHandler;
    private readonly ILogger _logger;

    protected ReceiverServiceBase(ITelegramBotClient botClient, TUpdateHandler updateHandler, ILogger logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            DropPendingUpdates = true,
            AllowedUpdates = []
        };

        var me = await _botClient.GetMe(stoppingToken);
        _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

        await _botClient.ReceiveAsync(_updateHandler, receiverOptions, stoppingToken);
    }
}