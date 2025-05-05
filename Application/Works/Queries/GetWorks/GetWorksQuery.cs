using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Works.Queries.GetWork;

namespace TasksManagement.Application.Works.Queries.GetWorks
{
    public record GetWorksQuery : IRequest<List<WorkResponse>>;
}
