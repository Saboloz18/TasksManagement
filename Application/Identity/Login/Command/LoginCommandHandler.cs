using MediatR;
using Microsoft.AspNetCore.Identity;
using TasksManagement.Domain.SystemUsers;
using TasksManagement.Application.Identity.Login.Command;
using TasksManagement.Application.Services.JwtService;
using TasksManagement.Application.Exceptions;

namespace TasksManagement.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly UserManager<SystemUser> _userManager; 
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(UserManager<SystemUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var token = _jwtTokenService.GenerateToken(user, role);

            return new LoginCommandResponse(token, user.UserName, role);
        }
    }

}