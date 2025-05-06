using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Works.Queries.GetWork;
using TasksManagement.Domain.Works;

namespace TasksManagement.Application.Users.Queries.GetUser
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WorkResponse> AssignedWorks { get; set; } = new List<WorkResponse>();
    }
}
