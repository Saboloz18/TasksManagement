using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.WorkAssignments.Commands.AssignWork
{
    public record AssignWorkCommand(int WorkId) : IRequest;
}
