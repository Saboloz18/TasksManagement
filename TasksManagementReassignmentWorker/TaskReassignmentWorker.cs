using MediatR;
using Quartz;
using TasksManagement.Application.WorkAssignments.Commands.AssignWork;
using TasksManagement.Domain.Works;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagementReassignmentWorker
{
    public class TaskReassignmentJob : IJob
    {

        private readonly IMediator _mediator;
        private readonly IWorkRepository _workRepository;
        private readonly ILogger<TaskReassignmentJob> _logger;

        public TaskReassignmentJob(IMediator mediator, IWorkRepository workRepository, ILogger<TaskReassignmentJob> logger)
        {
            _mediator = mediator;
            _workRepository = workRepository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Starting task reassignment at {Time}", DateTime.Now);

                var works = await _workRepository.GetAllAsync(context.CancellationToken);
                var worksToReassign = works
                    .Where(w => w.State != WorkState.Completed)
                    .ToList();

                foreach (var work in worksToReassign)
                {
                    await _mediator.Send(new AssignWorkCommand(work.Id), context.CancellationToken);
                }

                _logger.LogInformation("Reassigned {Count} works at {Time}", worksToReassign.Count, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reassigning works.");
            }
        }
    }
}
