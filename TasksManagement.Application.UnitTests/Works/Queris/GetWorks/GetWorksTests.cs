using AutoMapper;
using FluentAssertions;
using Moq;
using TasksManagement.Application.Works.Queries;
using TasksManagement.Application.Works.Queries.GetWorks;
using TasksManagement.Domain.Works;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.UnitTests.Works.Queris.GetWorks
{
    public class GetWorksQueryTests
    {
        private readonly Mock<IWorkRepository> _workRepositoryMock; 
        private readonly IMapper _mapper; 
        private readonly GetWorksQueryHandler _handler;

        public GetWorksQueryTests()
        {
            _workRepositoryMock = new Mock<IWorkRepository>();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new WorkMappingProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetWorksQueryHandler(_workRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_IfNoWorksExist_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetWorksQuery();
            _workRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Work>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_IfWorksExist_ShouldReturnMappedWorkResponses()
        {
            // Arrange
            var query = new GetWorksQuery();
            var works = new List<Work>
        {
            new Work { Id = 1, Title = "Work 1", State = WorkState.Waiting },
            new Work { Id = 2, Title = "Work 2", State = WorkState.InProgress }
        };
            _workRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(works);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].Title.Should().Be("Work 1");
            result[0].State.Should().Be(WorkState.Waiting.ToString());
            result[1].Id.Should().Be(2);
            result[1].Title.Should().Be("Work 2");
            result[1].State.Should().Be(WorkState.InProgress.ToString());
        }
    }
}
