namespace SecretaryTelegramAIBot.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}