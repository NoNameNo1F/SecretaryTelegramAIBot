using SecretaryTelegramAIBot.Application.Contracts;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public sealed class AddNoteCommand : CommandBase
{
    public long ChatId { get; set; }
    public string Brand { get; set; }
    public string Content { get; set; }

    public AddNoteCommand(long chatId, string brand, string content)
    {
        ChatId = chatId;
        Brand = brand;
        Content = content;
    }
}
