using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Services;
public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverService> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}