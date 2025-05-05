using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Persistance.Repositories.WorkAssignments;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.Works.Commands.DeleteWork
{
    public class DeleteWorkCommandHandler : IRequestHandler<DeleteWorkCommand>
    {
        private readonly IWorkRepository _workRepository;

        public DeleteWorkCommandHandler(IWorkRepository workRepository)
        {
            _workRepository = workRepository;
        }

        public async Task Handle(DeleteWorkCommand request, CancellationToken cancellationToken)
        {
            await _workRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
