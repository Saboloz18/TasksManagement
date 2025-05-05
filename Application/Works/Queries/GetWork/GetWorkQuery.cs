using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Works.Queries.GetWork
{
    public record GetWorkQuery(int Id) : IRequest<WorkResponse?>;
    public class GetWorkQueryValidator : AbstractValidator<GetWorkQuery>
    {
        public GetWorkQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
