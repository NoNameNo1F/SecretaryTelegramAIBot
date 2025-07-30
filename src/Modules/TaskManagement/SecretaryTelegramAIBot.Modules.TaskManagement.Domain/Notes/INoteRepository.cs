namespace SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes
{
    public interface INoteRepository
    {
        Task AddNoteAsync(Note note, CancellationToken cancellationToken);
        Task<IEnumerable<Note>> GetNotesAsync();
        Task<Note?> GetNoteByIdsAsync(NoteId nodeId, CancellationToken cancellationToken);
        Task<IEnumerable<Note>> GetNotesByChatIdAsync(long chatId, CancellationToken cancellationToken);
    }
}