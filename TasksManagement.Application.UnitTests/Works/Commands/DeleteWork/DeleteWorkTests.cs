using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Works.Commands.DeleteWork;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.Works.Commands.DeleteWork
{
    public class DeleteWorkCommandTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock; 
        private readonly DeleteWorkCommandHandler _handler;
        private readonly DeleteWorkCommandValidator _validator;

        public DeleteWorkCommandTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();
            _validator = new DeleteWorkCommandValidator();
            _handler = new DeleteWorkCommandHandler(_workRepositoryMock.Object);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var command = new DeleteWorkCommand(0);

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
            var command = new DeleteWorkCommand(-1);

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
            var command = new DeleteWorkCommand(1);

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
            var command = new DeleteWorkCommand(1);
            _workRepositoryMock.Setup(x => x.DeleteAsync(command.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _workRepositoryMock.Verify(x => x.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
