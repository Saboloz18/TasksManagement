using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.WorkAssignments;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.Works.Commands.UpdateWork
{
    public class UpdateWorkCommandHandler : IRequestHandler<UpdateWorkCommand>
    {
        private readonly IWorkRepository _workRepository;

        public UpdateWorkCommandHandler(IWorkRepository workRepository)
        {
            _workRepository = workRepository;
        }

        public async Task Handle(UpdateWorkCommand request, CancellationToken cancellationToken)
        {
            var work = await _workRepository.GetByIdAsync(request.Id, cancellationToken);
            if (work == null)
            {
                throw new NotFoundException("Work not found");
            }

            if ( work.Title == request.Title)
            {
                throw new AlreadyExistsException("Work title must be unique.");
            }

            work.Title = request.Title;
            work.State = request.State;
            work.CurrentUserId = request.CurrentUserId;

            await _workRepository.UpdateAsync(work, cancellationToken);
        }
    }
}
