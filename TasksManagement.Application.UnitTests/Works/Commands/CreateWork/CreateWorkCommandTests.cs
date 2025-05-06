using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using TasksManagement.Application.Works.Commands.CreateWork;
using TasksManagement.Domain.Users;
using TasksManagement.Domain.Works;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.Works.Commands.CreateWork
{
    public class CreateWorkCommandTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock; 
        private readonly Mock<IUserRepository> _userRepositoryMock; 
        private readonly CreateWorkCommandHandler _handler; 
        private readonly CreateWorkCommandValidator _validator;

        public CreateWorkCommandTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new CreateWorkCommandValidator();
            _handler = new CreateWorkCommandHandler(_workRepositoryMock.Object, _userRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfTitleIsEmpty_ShouldFailWithError()
        {
            // Arrange
            var command = new CreateWorkCommand("");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title is required.");
        }

        [Fact]
        public void Validate_IfTitleIsNull_ShouldFailWithError()
        {
            // Arrange
            var command = new CreateWorkCommand(null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title is required.");
        }

        [Fact]
        public void Validate_IfTitleExceeds100Characters_ShouldFailWithError()
        {
            // Arrange
            var longTitle = new string('A', 101); 
            var command = new CreateWorkCommand(longTitle);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title must not exceed 100 characters.");
        }

        [Fact]
        public void Validate_IfTitleIsValid_ShouldPass()
        {
            // Arrange
            var command = new CreateWorkCommand("Sample Work");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfWorkTitleExists_ShouldThrowAlreadyExistsException()
        {
            // Arrange
            var command = new CreateWorkCommand("Sample Work");
            _workRepositoryMock.Setup(x => x.ExistsByTitleAsync(command.Title, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfNoUsersAvailable_ShouldCreateWorkWithWaitingState()
        {
            // Arrange
            var command = new CreateWorkCommand("Sample Work");
            var createdWork = new Work { Id = 1, Title = "Sample Work", State = WorkState.Waiting };

            _workRepositoryMock.Setup(x => x.ExistsByTitleAsync(command.Title, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>());
            _workRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Work>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdWork);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            _workRepositoryMock.Verify(x => x.AddAsync(It.Is<Work>(w =>
                w.Title == "Sample Work" &&
                w.State == WorkState.Waiting &&
                w.CurrentUserId == null &&
                w.AssignmentHistory.Count == 0),
                It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_IfUserAvailable_ShouldCreateWorkWithInProgressStateAndAssignUser()
        {
            // Arrange
            var command = new CreateWorkCommand("Sample Work");
            var user = new User { Id = 1 };
            var users = new List<User> { user };
            var createdWork = new Work { Id = 1, Title = "Sample Work", State = WorkState.InProgress, CurrentUserId = 1 };

            _workRepositoryMock.Setup(x => x.ExistsByTitleAsync(command.Title, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);
            _workRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Work>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdWork);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            _workRepositoryMock.Verify(x => x.AddAsync(It.Is<Work>(w =>
                w.Title == "Sample Work" &&
                w.State == WorkState.InProgress &&
                w.CurrentUserId == 1 &&
                w.AssignmentHistory.Count == 1 &&
                w.AssignmentHistory[0].UserId == 1 &&
                w.AssignmentHistory[0].Cycle == 1),
                It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
