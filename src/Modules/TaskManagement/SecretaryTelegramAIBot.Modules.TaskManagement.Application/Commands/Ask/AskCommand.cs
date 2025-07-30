using SecretaryTelegramAIBot.Application.Contracts;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public sealed class AskCommand : CommandBase<string>
{
    public string Prompt { get; set; }

    public AskCommand(string prompt)
    {
        Prompt = prompt;
    }
}