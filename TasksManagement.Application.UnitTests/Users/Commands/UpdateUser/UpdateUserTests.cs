using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Users.Commands.UpdateUser;
using TasksManagement.Domain.Users;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.UnitTests.Users.Commands.UpdateUser
{
    public class UpdateUserCommandTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateUserCommandValidator _validator;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validator = new UpdateUserCommandValidator();

            _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var command = new UpdateUserCommand(0, "John Doe");

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
            var command = new UpdateUserCommand(-1, "John Doe");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfNameIsEmpty_ShouldFailWithError()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Validate_IfNameIsNull_ShouldFailWithError()
        {
            // Arrange
            var command = new UpdateUserCommand(1, null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Validate_IfNameExceeds50Characters_ShouldFailWithError()
        {
            // Arrange
            var longName = new string('A', 51); // 51 characters
            var command = new UpdateUserCommand(1, longName);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name must not exceed 50 characters.");
        }

        [Fact]
        public void Validate_IfIdAndNameAreValid_ShouldPass()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "John Doe");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfUserNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "John Doe");
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfNameIsSame_ShouldThrowAlreadyExistsException()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "John Doe");
            var existingUser = new User { Id = 1, Name = "John Doe" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfNameIsDifferent_ShouldUpdateUser()
        {
            // Arrange
            var command = new UpdateUserCommand(1, "Jane Doe");
            var existingUser = new User { Id = 1, Name = "John Doe" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingUser.Name.Should().Be("Jane Doe");
            _userRepositoryMock.Verify(x => x.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
