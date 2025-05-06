using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using TasksManagement.Application.Works.Queries;
using TasksManagement.Application.Works.Queries.GetWork;
using TasksManagement.Domain.Works;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.Works.Queris.GetWork
{
    public class GetWorkByIdQueryTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock; 
        private readonly IMapper _mapper; 
        private readonly GetWorkByIdQueryHandler _handler;
        private readonly GetWorkQueryValidator _validator;

        public GetWorkByIdQueryTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new WorkMappingProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _validator = new GetWorkQueryValidator();
            _handler = new GetWorkByIdQueryHandler(_workRepositoryMock.Object, _mapper);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var query = new GetWorkQuery(0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfIdIsNegative_ShouldFailWithError()
        {
            // Arrange
            var query = new GetWorkQuery(-1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_IfIdIsPositive_ShouldPass()
        {
            // Arrange
            var query = new GetWorkQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfWorkNotFound_ShouldReturnNull()
        {
            // Arrange
            var query = new GetWorkQuery(1);
            _workRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Work?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_IfWorkFound_ShouldReturnMappedWorkResponse()
        {
            // Arrange
            var query = new GetWorkQuery(1);
            var work = new Work { Id = 1, Title = "Sample Work", State = WorkState.Waiting };
            var workResponse = new WorkResponse { Id = 1, Title = "Sample Work", State = WorkState.Waiting.ToString() };
            _workRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(work);
            // AutoMapper will handle the mapping, so no explicit mock setup is needed here

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(workResponse);
        }
    }
}
