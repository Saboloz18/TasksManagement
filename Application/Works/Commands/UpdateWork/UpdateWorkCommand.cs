﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;

namespace TasksManagement.Application.Works.Commands.UpdateWork
{
    public record UpdateWorkCommand(int Id, string Title) : IRequest;

    public class UpdateWorkCommandValidator : AbstractValidator<UpdateWorkCommand>
    {
        public UpdateWorkCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}
