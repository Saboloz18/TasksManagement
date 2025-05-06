using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments
{
    public class WorkAssignmentResponse
    {
        public int Id { get; set; }
        public int WorkId { get; set; }
        public string WorkTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Cycle { get; set; }
    }
}
