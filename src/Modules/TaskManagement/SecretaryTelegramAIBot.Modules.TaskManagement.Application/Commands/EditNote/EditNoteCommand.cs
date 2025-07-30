using SecretaryTelegramAIBot.Application.Contracts;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public sealed class EditNoteCommand : CommandBase
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string Content { get; set; }
    public string Brand { get; set; }

    public EditNoteCommand(int id, long chatId, string content, string brand)
    {
        Id = id;
        ChatId = chatId;
        Content = content;
        Brand = brand;
    }
}
