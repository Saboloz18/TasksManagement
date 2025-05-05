using MediatR;
using Microsoft.AspNetCore.Mvc;
using TasksManagement.Application.Works.Commands.CreateWork;
using TasksManagement.Application.Works.Commands.DeleteWork;
using TasksManagement.Application.Works.Commands.UpdateWork;
using TasksManagement.Application.Works.Queries.GetWork;
using TasksManagement.Application.Works.Queries.GetWorks;

namespace TasksManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorksController : ControllerBase
    {
        private readonly IMediator _mediator; private readonly ILogger _logger;

        public WorksController(IMediator mediator, ILogger<WorksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorks(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all works at {Time}", DateTime.UtcNow);
            var result = await _mediator.Send(new GetWorksQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching work with ID {Id} at {Time}", id, DateTime.UtcNow);
            var result = await _mediator.Send(new GetWorkQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWork([FromBody] CreateWorkCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWork(int id, [FromBody] UpdateWorkCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateWorkCommand(id, command.Title, command.State, command.CurrentUserId), cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWork(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting work with ID {Id} at {Time}", id, DateTime.UtcNow);
            await _mediator.Send(new DeleteWorkCommand(id), cancellationToken);
            return Ok();
        }
    }
}