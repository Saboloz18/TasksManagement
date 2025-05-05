using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Users.Queries.GetUser
{
    public record GetUserQuery(int Id) : IRequest<UserResponse?>;

    public class GetUserByIdQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
