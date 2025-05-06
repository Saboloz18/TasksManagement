using System.Collections.Generic;
using TasksManagement.Domain.Works;
using TasksManagement.Domain.Shared;

namespace TasksManagement.Domain.Users
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public List<Work> AssignedWork { get; set; } = new List<Work>();
    }
}