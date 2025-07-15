using Microsoft.EntityFrameworkCore;
using SecretaryTelegramAIBot.Domain.Entities;
using SecretaryTelegramAIBot.Domain.Repositories;

namespace SecretaryTelegramAIBot.Infrastructure.Repositories;

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
        return await _dbContext.Notes.ToListAsync();
    }

    public async Task<Note?> GetNoteByIdsAsync(NoteId nodeId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == nodeId, cancellationToken);
    }
}