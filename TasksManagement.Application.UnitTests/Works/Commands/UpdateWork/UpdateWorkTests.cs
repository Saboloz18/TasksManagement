using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Works.Commands.UpdateWork;
using TasksManagement.Domain.Works;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.Works.Commands.UpdateWork
{
    public class UpdateWorkCommandTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock;
        private readonly UpdateWorkCommandHandler _handler;
        private readonly UpdateWorkCommandValidator _validator;

        public UpdateWorkCommandTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();
            _validator = new UpdateWorkCommandValidator();
            _handler = new UpdateWorkCommandHandler(_workRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var command = new UpdateWorkCommand(0, "Sample Work");

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
            var command = new UpdateWorkCommand(-1, "Sample Work");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfTitleIsEmpty_ShouldFailWithError()
        {
            // Arrange
            var command = new UpdateWorkCommand(1, "");

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
            var command = new UpdateWorkCommand(1, null);

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
            var longTitle = new string('A', 101); // 101 characters
            var command = new UpdateWorkCommand(1, longTitle);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title must not exceed 100 characters.");
        }

        [Fact]
        public void Validate_IfIdAndTitleAreValid_ShouldPass()
        {
            // Arrange
            var command = new UpdateWorkCommand(1, "Sample Work");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfWorkNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateWorkCommand(1, "Sample Work");
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Work?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfTitleIsSame_ShouldThrowAlreadyExistsException()
        {
            // Arrange
            var command = new UpdateWorkCommand(1, "Sample Work");
            var existingWork = new Work { Id = 1, Title = "Sample Work" };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingWork);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_IfTitleIsDifferent_ShouldUpdateWork()
        {
            // Arrange
            var command = new UpdateWorkCommand(1, "Updated Work");
            var existingWork = new Work { Id = 1, Title = "Sample Work" };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingWork);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingWork.Title.Should().Be("Updated Work");
            _workRepositoryMock.Verify(x => x.UpdateAsync(existingWork, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
