using AutoMapper;
using FluentAssertions;
using Moq;
using TasksManagement.Application.WorkAssignments.Queries;
using TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments;
using TasksManagement.Domain.WorkAssignments;
using TasksManagement.Persistance.Repositories.WorkAssignments;


namespace TasksManagement.Application.UnitTests.WorAssignments.Queries.GetWorkAssignments
{
    public class GetWorkAssignmentQueryTests
    {
        private readonly Mock<IWorkAssignmentRepository> _workAssignmentRepositoryMock;
        private readonly IMapper _mapper; 
        private readonly GetAssignedWorksQueryHandler _handler;

        public GetWorkAssignmentQueryTests()
        {
            _workAssignmentRepositoryMock = new Mock<IWorkAssignmentRepository>();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new WorkAssignmentMappingProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetAssignedWorksQueryHandler(_workAssignmentRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_IfNoWorkAssignmentsExist_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetWorkAssignmentQuery();
            _workAssignmentRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WorkAssignment>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_IfWorkAssignmentsExist_ShouldReturnMappedWorkAssignmentResponses()
        {
            // Arrange
            var query = new GetWorkAssignmentQuery();
            var workAssignments = new List<WorkAssignment>
        {
            new WorkAssignment { Id = 1, WorkId = 1, UserId = 1, Cycle = 1 },
            new WorkAssignment { Id = 2, WorkId = 2, UserId = 2, Cycle = 2 }
        };
            _workAssignmentRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(workAssignments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].WorkId.Should().Be(1);
            result[0].UserId.Should().Be(1);
            result[0].Cycle.Should().Be(1);
            result[1].Id.Should().Be(2);
            result[1].WorkId.Should().Be(2);
            result[1].UserId.Should().Be(2);
            result[1].Cycle.Should().Be(2);
        }
    }
}
