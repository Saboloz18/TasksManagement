using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManagement.Application.Users.Commands.CreateUser;
using TasksManagement.Application.Users.Commands.DeleteUser;
using TasksManagement.Application.Users.Commands.UpdateUser;
using TasksManagement.Application.Users.Queries.GetUser;
using TasksManagement.Application.Users.Queries.GetUsers;

namespace TasksManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get List of Users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetUsersQuery(), cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Get an user based on Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserQuery(id), cancellationToken);
            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return Ok(userId);
        }

        /// <summary>
        /// Update an existing user's name
        /// </summary>
        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete an user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
            return Ok();
        }
    }
}