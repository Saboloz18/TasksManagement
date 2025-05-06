using AutoMapper;
using FluentAssertions;
using Moq;
using TasksManagement.Application.Users.Queries.GetUser;
using TasksManagement.Application.Users.Queries.GetUsers;
using TasksManagement.Domain.Users;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.UnitTests.Users.Queries.GetUsers
{
    public class GetUsersQueryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetUsersQueryHandler _handler;

        public GetUsersQueryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserMappingProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetUsersQueryHandler(_userRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_IfNoUsersExist_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetUsersQuery();
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_IfUsersExist_ShouldReturnMappedUserResponses()
        {
            // Arrange
            var query = new GetUsersQuery();
            var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe" },
            new User { Id = 2, Name = "Jane Doe" }
        };
            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].Name.Should().Be("John Doe");
            result[1].Id.Should().Be(2);
            result[1].Name.Should().Be("Jane Doe");
        }
    }
}
