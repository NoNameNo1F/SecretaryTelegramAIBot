using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretaryTelegramAIBot.Modules.TaskManagement.Application.Commands;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Enums;

namespace SecretaryTelegramAIBot.API.Controllers
{
    [ApiController]
    [Route("api/v1/note")]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;
    
        public NoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] AddNoteCommand request,
            CancellationToken cancellation = default)
        {
            await _mediator.Send(request, cancellation);
        
            return Created();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportNotes(int type)
        {
            var data = await _mediator.Send(new ExportNoteCommand(ERange.DAILY,ExportType.TEXT));
        
            return Ok(data);
        }
    }
}