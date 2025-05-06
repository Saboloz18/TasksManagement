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

namespace TasksManagement.Application.WorkAssignments.Commands.AssignWork
{
    public class AssignWorkCommandHandler : IRequestHandler<AssignWorkCommand>
    {
        private readonly IWorkRepository _workRepository;
        private readonly IUserRepository _userRepository;
        public AssignWorkCommandHandler(IWorkRepository workRepository, IUserRepository userRepository)
        {
            _workRepository = workRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(AssignWorkCommand request, CancellationToken cancellationToken)
        {
            var work = await _workRepository.GetByIdAsync(request.WorkId, cancellationToken);
            if (work == null)
            {
                throw new NotFoundException("Work not found.");
            }

            var users = await _userRepository.GetAllAsync(cancellationToken);
            if (!users.Any())
            {
                work.State = WorkState.Waiting;
                work.UpdateDate = DateTime.Now;
                work.CurrentUserId = null;
                await _workRepository.UpdateAsync(work, cancellationToken);
                return;
            }

            var assignedUserIds = work.AssignmentHistory.Select(wa => wa.UserId).Distinct().ToHashSet();
            var allUsersAssigned = users.All(u => assignedUserIds.Contains(u.Id));
            if (allUsersAssigned)
            {
                work.State = WorkState.Completed;
                work.UpdateDate = DateTime.Now;
                work.CurrentUserId = null;
                await _workRepository.UpdateAsync(work, cancellationToken);
                return;
            }

            var currentUserId = work.CurrentUserId;
            var previousUserId = work.AssignmentHistory.Count >= 2
                ? work.AssignmentHistory.OrderByDescending(wa => wa.Cycle).Skip(1).FirstOrDefault()?.UserId
                : null;

            var eligibleUsers = users
                .Where(u => u.Id != currentUserId && u.Id != previousUserId) 
                .Where(u => !work.AssignmentHistory.Any(wa => wa.UserId == u.Id) ||
                           work.AssignmentHistory.Where(wa => wa.UserId == u.Id).Max(wa => wa.Cycle) <= work.AssignmentHistory.Max(wa => wa.Cycle) - 2) 
                .ToList();

            if (!eligibleUsers.Any())
            {
                work.State = WorkState.Waiting;
                work.CurrentUserId = null;
            }
            else
            {
                var random = new Random();
                var nextUser = eligibleUsers[random.Next(eligibleUsers.Count)];
                work.CurrentUserId = nextUser.Id;
                work.State = WorkState.InProgress;
                work.AssignmentHistory.Add(new WorkAssignment
                {
                    UserId = nextUser.Id,
                    Cycle = work.AssignmentHistory.Any() ? work.AssignmentHistory.Max(wa => wa.Cycle) + 1 : 1
                });
            }
            work.UpdateDate = DateTime.Now;
            await _workRepository.UpdateAsync(work, cancellationToken);
        }
    }
}
