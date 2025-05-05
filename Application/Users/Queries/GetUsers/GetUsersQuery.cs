using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Users.Queries.GetUser;

namespace TasksManagement.Application.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<List<UserResponse>>;
}
