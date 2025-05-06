using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Identity.Register.Command
{
    public record RegisterUserCommand(string Username, string Password, string Role) : IRequest<string>;
}
