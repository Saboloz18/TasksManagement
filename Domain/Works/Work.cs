using TasksManagement.Domain.Shared;
using TasksManagement.Domain.Users;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Domain.Works
{
    public class Work : EntityBase
    {
        public string Title { get; set; } = string.Empty; 
        public WorkState State { get; set; }
        public int? CurrentUserId { get; set; }
        public User? CurrentUser { get; set; } 
        public List<WorkAssignment> AssignmentHistory { get; set; } = new();
    }
}
