using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using TasksManagement.Application.Users.Commands.CreateUser;
using TasksManagement.Domain.Users;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.UnitTests.Users.Commands.CreateUser
{
    public class CreateUserCommandTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock; 
        
        private readonly CreateUserCommandValidator _validator;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new CreateUserCommandValidator();
            _handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfNameIsEmpty_ShouldFailWithError()
        {
            // Arrange
            var command = new CreateUserCommand("");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("User name is required");
        }

        [Fact]
        public void Validate_IfNameIsNull_ShouldFailWithError()
        {
            // Arrange
            var command = new CreateUserCommand(null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("User name is required");
        }

        [Fact]
        public void Validate_IfNameExceeds50Characters_ShouldFailWithError()
        {
            // Arrange
            var longName = new string('A', 51); // 51 characters
            var command = new CreateUserCommand(longName);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("User name must not exceed 50 characters.");
        }

        [Fact]
        public void Validate_IfNameIsValid_ShouldPass()
        {
            // Arrange
            var command = new CreateUserCommand("John Doe");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfUserExists_ShouldThrowAlreadyExistsException()
        {
            // Arrange
            var command = new CreateUserCommand("John Doe");
            _userRepositoryMock.Setup(x => x.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
           
            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfUserDoesNotExist_ShouldCreateUserAndReturnId()
        {
            // Arrange
            var command = new CreateUserCommand("John Doe");
            var createdUser = new User { Id = 1, Name = "John Doe" };

            _userRepositoryMock.Setup(x => x.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.AddAsync(It.Is<User>(u => u.Name == command.Name), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(1);
            _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u => u.Name == "John Doe"), It.IsAny<CancellationToken>()), Times.Once());
        }
    }

}
