using SecretaryTelegramAIBot.Domain.SeedWork;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Brands;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes
{
    public class Note : IAggregateRoot, ITrackable, ISoftDelete
    {
        public NoteId Id { get; private set; }
        public long ChatId { get; private set; }
        public string Content { get; private set;}
        public NoteStatus Status { get; private set; }
        public BrandId BrandId { get; private set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        private Note() { }
        
        private Note(long chatId, string content, BrandId brandId, DateTime createdAt)
        {
            Id = new NoteId(0);
            ChatId = chatId;
            Content = content;
            BrandId = brandId;
            CreatedAt = createdAt;
        }
    
        public static Note CreateNew(long chatId, BrandId brandId, string content)
        {
            var note = new Note(chatId, content, brandId, DateTime.Now);
            note.Status = NoteStatus.Created;
            
            return note;
        }
    
        public void UpdateNote(string content)
        {
            Content = content;
            UpdatedAt = DateTime.Now;
        }
        
        public void ChangeState(NoteStatus status)
        {
            Status = status;
        }
    }
}