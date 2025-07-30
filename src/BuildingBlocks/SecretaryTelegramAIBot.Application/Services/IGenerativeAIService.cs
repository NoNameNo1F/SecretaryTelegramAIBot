namespace SecretaryTelegramAIBot.Application.Services
{
    public interface IGenerativeAIService
    {
        Task<string> GenerateText(string prompt, CancellationToken cancellationToken);
    }
}