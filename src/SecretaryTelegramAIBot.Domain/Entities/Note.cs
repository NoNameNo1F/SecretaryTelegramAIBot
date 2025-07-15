using SecretaryTelegramAIBot.Domain.SeedWork;

namespace SecretaryTelegramAIBot.Domain.Entities;

public class Note : IAggregateRoot
{
    public NoteId Id { get; set; }
    public long ChatId { get; set; }
    public string Content { get; set;}
    public string Brand { get; set;}
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set;}
    
    private Note()
    {
    }

    private Note(long chatId, string content, string brand, DateTime createdAt)
    {
        Id = new NoteId();
        ChatId = chatId;
        Content = content;
        Brand = brand;
        CreatedAt = createdAt;
    }
    
    public static Note CreateNew(long chatId, string content, string brand)
    {
        return new Note(chatId, content, brand, DateTime.Now);
    }
    
    public void Update(string content, string brand)
    {
        Content = content;
        Brand = brand;
        UpdatedAt = DateTime.Now;
    }
}