using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Domain.Repositories;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public class EditNoteCommandHandler : ICommandHandler<EditNoteCommand>
{
    private readonly INoteRepository _noteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditNoteCommandHandler(INoteRepository noteRepository, IUnitOfWork unitOfWork)
    {
        _noteRepository = noteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EditNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetNoteByIdsAsync(new NoteId(request.Id), cancellationToken);
        if (note is null)
        {
            throw new ApplicationException($"Note with id {request.Id} not found");
        }
    
        note.Update(request.Content, request.Brand);
        await _unitOfWork.CommitAsync();
    }
}