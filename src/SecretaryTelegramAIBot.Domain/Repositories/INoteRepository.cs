using SecretaryTelegramAIBot.Domain.Entities;

namespace SecretaryTelegramAIBot.Domain.Repositories;

public interface INoteRepository
{
    Task AddNoteAsync(Note note, CancellationToken cancellationToken);
    Task<IEnumerable<Note>> GetNotesAsync();
    Task<Note?> GetNoteByIdsAsync(NoteId nodeId, CancellationToken cancellationToken);
}