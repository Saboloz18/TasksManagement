using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Users.Commands.DeleteUser;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.UnitTests.Users.Commands.DeleteUser
{
    public class DeleteUserCommandTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteUserCommandValidator _validator;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new DeleteUserCommandValidator();
            _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var command = new DeleteUserCommand(0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfIdIsNegative_ShouldFailWithError()
        {
            // Arrange
            var command = new DeleteUserCommand(-1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfIdIsPositive_ShouldPass()
        {
            // Arrange
            var command = new DeleteUserCommand(1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfIdIsValid_ShouldCallDeleteAsync()
        {
            // Arrange
            var command = new DeleteUserCommand(1);
            _userRepositoryMock.Setup(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);           

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
