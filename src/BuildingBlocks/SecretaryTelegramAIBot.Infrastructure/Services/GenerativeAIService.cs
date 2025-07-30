using GenerativeAI.Types;
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
        var modelConfig = new GenerationConfig
        {
            MaxOutputTokens = 1000,
            Temperature = 0.9f,
            TopP = 1.0f,
            TopK = 1
        };
        
        var contentRequest = new GenerateContentRequest
        {
            GenerationConfig = modelConfig,

            Contents = new List<Content>
            {
                new Content(prompt, "user")
            }
        };
            
        var model = _generativeAiService.CreateInstance();
        var response = await model.GenerateContentAsync(contentRequest, cancellation);

        return response.Text;
    }
}