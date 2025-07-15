using SecretaryTelegramAIBot.Domain.Enums;

namespace SecretaryTelegramAIBot.Application.Commands;

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