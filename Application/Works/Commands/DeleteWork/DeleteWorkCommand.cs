using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Works.Commands.DeleteWork
{
    public record DeleteWorkCommand(int Id) : IRequest;

    public class DeleteWorkCommandValidator : AbstractValidator<DeleteWorkCommand>
    {
        public DeleteWorkCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
