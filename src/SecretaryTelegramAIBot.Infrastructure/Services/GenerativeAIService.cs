using GenerativeAI.Web;

namespace SecretaryTelegramAIBot.Infrastructure.Services;

public class GenerativeAIService
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