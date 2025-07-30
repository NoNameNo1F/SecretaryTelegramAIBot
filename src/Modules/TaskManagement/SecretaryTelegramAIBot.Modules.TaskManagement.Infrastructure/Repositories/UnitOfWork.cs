using SecretaryTelegramAIBot.Domain.Repositories;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly TelegramBotDbContext _dbContext;

    public UnitOfWork(TelegramBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}