using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Exceptions;
using TasksManagement.Domain.SystemUsers;

namespace TasksManagement.Application.Identity.Register.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly UserManager<SystemUser> _userManager;

        public RegisterUserCommandHandler(UserManager<SystemUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new SystemUser
            {
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new RegistrationException($"Failed to register user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(user, request.Role);
            return user.Id;
        }
    }
}
