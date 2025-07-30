namespace SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

public record NoteStatus
{
    public string Code { get; init; }

    private NoteStatus(string code)
    {
        Code = code;
    }

    public static readonly NoteStatus Created = new(nameof(Created));
    public static readonly NoteStatus OnProcess = new(nameof(OnProcess));
    public static readonly NoteStatus Done = new(nameof(Done));
    public static readonly NoteStatus Cancelled = new(nameof(Cancelled));

    public static NoteStatus Of(string code)
    {
        return code switch
        {
            nameof(Created) => Created,
            nameof(OnProcess) => OnProcess,
            nameof(Done) => Done,
            nameof(Cancelled) => Cancelled,
            _ => throw new ApplicationException($"Status with code '{code}' not found.")
        };
    }
}