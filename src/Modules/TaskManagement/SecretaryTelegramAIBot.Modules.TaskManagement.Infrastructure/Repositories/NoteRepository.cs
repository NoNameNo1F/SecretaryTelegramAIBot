using Microsoft.EntityFrameworkCore;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Repositories;
public class NoteRepository : INoteRepository
{
    private readonly TelegramBotDbContext _dbContext;
    
    public NoteRepository(TelegramBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddNoteAsync(Note note, CancellationToken cancellationToken)
    {
        await _dbContext.Notes.AddAsync(note, cancellationToken);
    }

    public async Task<IEnumerable<Note>> GetNotesAsync()
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Note?> GetNoteByIdsAsync(NoteId nodeId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == nodeId, cancellationToken);
    }

    public async Task<IEnumerable<Note>> GetNotesByChatIdAsync(long chatId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}