using FluentValidation;
using MediatR;


namespace TasksManagement.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(string Name) : IRequest<int>;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required")
                .MaximumLength(50).WithMessage("User name must not exceed 50 characters.");
        }
    }
}
