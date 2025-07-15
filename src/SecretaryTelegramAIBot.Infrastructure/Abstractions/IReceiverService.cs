namespace SecretaryTelegramAIBot.Infrastructure.Abstractions;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}