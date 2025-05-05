using TasksManagement.Domain.Shared;
using TasksManagement.Domain.Users;


namespace TasksManagement.Domain.WorkAssignments
{
    public class WorkAssignment : EntityBase
    {
        public int WorkId { get; set; } 
        public int UserId { get; set; }
        public User User { get; set; } = null!; 
        public int Cycle { get; set; }
    }
}
