using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Domain.Repositories;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
public class AddNoteCommandHandler : ICommandHandler<AddNoteCommand>
{
    private readonly INoteRepository _noteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddNoteCommandHandler(INoteRepository noteRepository, IUnitOfWork unitOfWork)
    {
        _noteRepository = noteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        // find brand => get brandId
        var note = Note.CreateNew(request.ChatId, request.Brand, request.Content);
        await _noteRepository.AddNoteAsync(note, cancellationToken);
        await _unitOfWork.CommitAsync();
    }
}
