using SecretaryTelegramAIBot.Domain.Entities;
using SecretaryTelegramAIBot.Domain.Repositories;

namespace SecretaryTelegramAIBot.Application.Commands;

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
        var note = Note.CreateNew(request.ChatId, request.Content, request.Brand);
        await _noteRepository.AddNoteAsync(note, cancellationToken);
        await _unitOfWork.CommitAsync();
    }
}