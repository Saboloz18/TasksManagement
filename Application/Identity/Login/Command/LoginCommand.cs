using MediatR;

namespace TasksManagement.Application.Identity.Login.Command
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginCommandResponse>;
}
