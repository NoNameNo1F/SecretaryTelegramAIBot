using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Application.Services;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public class AskCommandHandler : ICommandHandler<AskCommand, string>
{
    private readonly IGenerativeAIService _aiService;

    public AskCommandHandler(IGenerativeAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<string> Handle(AskCommand request, CancellationToken cancellationToken)
    {
        var response = await _aiService.GenerateText(request.Prompt, cancellationToken);

        return response;
    }
}