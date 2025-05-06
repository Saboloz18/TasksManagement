using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.WorkAssignments.Commands.AssignWork;
using TasksManagement.Domain.Users;
using TasksManagement.Domain.WorkAssignments;
using TasksManagement.Domain.Works;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.WorAssignments.Commands
{
    public class AssignWorkCommandTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock; 
        private readonly AssignWorkCommandHandler _handler;

        public AssignWorkCommandTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new AssignWorkCommandHandler(_workRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_IfWorkNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new AssignWorkCommand(1);
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.WorkId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Work)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfNoUsersAvailable_ShouldSetWorkToWaiting()
        {
            // Arrange
            var command = new AssignWorkCommand(1);
            var work = new Work { Id = 1, State = WorkState.InProgress, CurrentUserId = 1, AssignmentHistory = new List<WorkAssignment>() };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.WorkId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(work);
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            work.State.Should().Be(WorkState.Waiting);
            work.CurrentUserId.Should().BeNull();
            work.UpdateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            _workRepositoryMock.Verify(x => x.UpdateAsync(work, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_IfEligibleUsersExistAndPreviousUserExists_ShouldAssignNewUser()
        {
            // Arrange
            var command = new AssignWorkCommand(1);
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 2 };
            var user3 = new User { Id = 3 };
            var users = new List<User> { user1, user2, user3 };
            var work = new Work
            {
                Id = 1,
                State = WorkState.InProgress,
                CurrentUserId = 2, // Current user is user2
                AssignmentHistory = new List<WorkAssignment>
            {
                new WorkAssignment { UserId = 1, Cycle = 1 }, // Previous user is user1
                new WorkAssignment { UserId = 2, Cycle = 2 }  // Current user
            }
            };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.WorkId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(work);
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            work.State.Should().Be(WorkState.InProgress);
            work.CurrentUserId.Should().Be(3); // Only user3 is eligible (not user1 or user2)
            work.AssignmentHistory.Should().HaveCount(3);
            work.AssignmentHistory.Last().UserId.Should().Be(3);
            work.AssignmentHistory.Last().Cycle.Should().Be(3);
            work.UpdateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            _workRepositoryMock.Verify(x => x.UpdateAsync(work, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_IfEligibleUsersExistAndNoPreviousUser_ShouldAssignNewUser()
        {
            // Arrange
            var command = new AssignWorkCommand(1);
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 2 };
            var users = new List<User> { user1, user2 };
            var work = new Work
            {
                Id = 1,
                State = WorkState.Waiting,
                CurrentUserId = null, // No current user
                AssignmentHistory = new List<WorkAssignment>()
            };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.WorkId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(work);
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            work.State.Should().Be(WorkState.InProgress);
            work.CurrentUserId.Should().BeOneOf(1, 2); // Randomly assigns user1 or user2
            work.AssignmentHistory.Should().HaveCount(1);
            work.AssignmentHistory.Last().UserId.Should().Be(work.CurrentUserId);
            work.AssignmentHistory.Last().Cycle.Should().Be(1);
            work.UpdateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            _workRepositoryMock.Verify(x => x.UpdateAsync(work, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}

