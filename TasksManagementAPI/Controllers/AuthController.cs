using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManagement.Application.Identity.Login.Command;
using TasksManagement.Application.Identity.Register.Command;

namespace TasksManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Logs in a user and returns an authentication token.
        /// </summary>
        /// <remarks>
        /// This endpoint authenticates a user with the provided credentials and returns a JWT token if successful.
        /// Use this token in the Authorization header for subsequent requests (e.g., "Bearer {token}").
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Registers a new user, only accesible by an user with admin role
        /// </summary>
  
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Register), new { id = userId }, null);
        }
    }

}

