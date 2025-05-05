using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.WorkAssignments;
using TasksManagement.Domain.Works;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.Works.Commands.CreateWork
{
    public class CreateWorkCommandHandler : IRequestHandler<CreateWorkCommand, int>
    {
        private readonly IWorkRepository _workRepository;
        private readonly IUserRepository _userRepository;

        public CreateWorkCommandHandler(IWorkRepository workRepository, IUserRepository userRepository)
        {
            _workRepository = workRepository;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateWorkCommand request, CancellationToken cancellationToken)
        {
            if (await _workRepository.ExistsByTitleAsync(request.Title, cancellationToken))
                throw new AlreadyExistsException("Work title must be unique.");

            var work = new Work { Title = request.Title, State = WorkState.Waiting };
            var users = await _userRepository.GetAllAsync(cancellationToken);
            var availableUser = users.FirstOrDefault();

            if (availableUser != null)
            {
                work.CurrentUserId = availableUser.Id;
                work.State = WorkState.InProgress;
                work.AssignmentHistory.Add(new WorkAssignment { UserId = availableUser.Id, Cycle = 1 });
            }

            var createdWork = await _workRepository.AddAsync(work, cancellationToken);
            return createdWork.Id;
        }
    }
}
