using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Works.Queries.GetWork
{
    public class WorkResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int? CurrentUserId { get; set; }
        public string? CurrentUserName { get; set; }
    }
}
