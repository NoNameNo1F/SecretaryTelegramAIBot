using ClosedXML.Excel;
using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Enums;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public class ExportNoteCommandHandler : ICommandHandler<ExportNoteCommand, object>
{
    private readonly INoteRepository _noteRepository;

    public ExportNoteCommandHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<object> Handle(ExportNoteCommand request, CancellationToken cancellationToken)
    {
        var notes = await _noteRepository.GetNotesAsync();
        notes = FilterNotesByRange(notes, request.Range);
    
        switch (request.Type)
        {
            case ExportType.TEXT:
                return ExportToText(notes);
        
            case ExportType.EXCEL:
                return ExportToFile(notes);
        
            default:
                return ExportToText(notes);
        }
    }

    private IEnumerable<Note> FilterNotesByRange(IEnumerable<Note> notes, ERange range)
    {
        var now = DateTime.Today;
        return range switch
        {
            ERange.DAILY => notes.Where(n => n.CreatedAt.Date == now),
            ERange.WEEKLY => notes.Where(n => n.CreatedAt.Date >= now.AddDays(-7) && n.CreatedAt.Date <= now),
            ERange.MONTHLY => notes.Where(n => n.CreatedAt.Date >= now.AddDays(-30) && n.CreatedAt.Date <= now),
            _ => notes
        };
    }

    private string ExportToText(IEnumerable<Note> notes)
    {
        if (!notes.Any())
            return "No notes found for the specified period.";

        return string.Join("\n", notes.Select(n => $"[{n.CreatedAt}] {n.Brand}: {n.Content}"));
    }

    private byte[] ExportToFile(IEnumerable<Note> notes)
    {
        var today = DateTime.Today.Date;
    
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"Notes");
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Brand Name";
        worksheet.Cell(1, 3).Value = "Content";
        worksheet.Cell(1, 4).Value = "Timestamp";
    
        var currentRow = 2;
        foreach (var note in notes)
        {
            worksheet.Cell(currentRow, 1).Value = note.Id.Value;
            worksheet.Cell(currentRow, 2).Value = note.Brand;
            worksheet.Cell(currentRow, 3).Value = note.Content;
            worksheet.Cell(currentRow, 4).Value = note.CreatedAt;
            currentRow++;
        }
    
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}