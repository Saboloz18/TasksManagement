using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Users.Commands.UpdateUser;
using TasksManagement.Application.Users.Queries.GetUser;
using TasksManagement.Application.Users.Queries.GetUsers;
using TasksManagement.Domain.Users;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserByIdQueryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper; 
        private readonly GetUserByIdQueryHandler _handler;
        private readonly GetUserByIdQueryValidator _validator;

        public GetUserByIdQueryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserMappingProfile()); });
            _mapper= mapperConfig.CreateMapper();
            _validator = new GetUserByIdQueryValidator();
            _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object, _mapper);
        }

        // Validator Tests
        [Fact]
        public void Validate_IfIdIsZero_ShouldFailWithError()
        {
            // Arrange
            var query = new GetUserQuery(0);

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
            var query = new GetUserQuery(-1);

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
            var query = new GetUserQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        // Handler Tests
        [Fact]
        public async Task Handle_IfUserNotFound_ShouldReturnNull()
        {
            // Arrange
            var query = new GetUserQuery(1);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_IfUserFound_ShouldReturnMappedUserResponse()
        {
            // Arrange
            var query = new GetUserQuery(1);
            var user = new User { Id = 1, Name = "John Doe" };
            var userResponse = new UserResponse { Id = 1, Name = "John Doe" };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(userResponse);
        }
    }
}
