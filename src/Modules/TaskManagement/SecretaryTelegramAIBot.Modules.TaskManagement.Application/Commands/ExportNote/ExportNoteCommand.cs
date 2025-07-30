using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Enums;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public sealed class ExportNoteCommand : CommandBase<object>
{
    public ERange Range { get; set; }
    public ExportType Type { get; set; }

    public ExportNoteCommand(ERange range, ExportType type)
    {
        Range = range;
        Type = type;
    }
}