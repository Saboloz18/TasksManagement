using TasksManagement.Domain.Shared;
using TasksManagement.Domain.Works;

namespace TasksManagement.Domain.Users
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<Work> AssignedWork { get; set; }
    }
}
