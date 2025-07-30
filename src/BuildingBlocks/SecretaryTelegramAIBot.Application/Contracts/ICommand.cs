using MediatR;

namespace SecretaryTelegramAIBot.Application.Contracts
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}