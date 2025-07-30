namespace SecretaryTelegramAIBot.Application.Contracts
{
    public abstract class CommandBase : ICommand
    {
    }

    public abstract class CommandBase<TResponse> : ICommand<TResponse>
    {
    }
}