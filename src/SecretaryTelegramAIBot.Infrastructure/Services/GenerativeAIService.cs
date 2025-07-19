using GenerativeAI.Web;
using SecretaryTelegramAIBot.Application.Services;

namespace SecretaryTelegramAIBot.Infrastructure.Services;

public class GenerativeAIService : IGenerativeAIService
{
    private readonly IGenerativeAiService _generativeAiService;

    public GenerativeAIService(IGenerativeAiService generativeAiService)
    {
        _generativeAiService = generativeAiService;
    }

    public async Task<string> GenerateText(string prompt, CancellationToken cancellation)
    {
        var response = await _generativeAiService.CreateInstance().GenerateContentAsync(prompt, cancellation);

        return response.Text;
    }
}