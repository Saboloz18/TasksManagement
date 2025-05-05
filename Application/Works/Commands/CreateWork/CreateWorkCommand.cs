using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Works.Commands.CreateWork
{
    public record CreateWorkCommand(string Title) : IRequest<int>;

    public class CreateWorkCommandValidator : AbstractValidator<CreateWorkCommand>
    {
        public CreateWorkCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}
