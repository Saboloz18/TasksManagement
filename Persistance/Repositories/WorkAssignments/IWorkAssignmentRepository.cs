using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Persistance.Repositories.WorkAssignments
{
    public interface IWorkAssignmentRepository
    {
        Task<WorkAssignment> AddAsync(WorkAssignment workAssignment, CancellationToken cancellationToken);
        Task<WorkAssignment?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<WorkAssignment>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<WorkAssignment>> GetByWorkIdAsync(int workId, CancellationToken cancellationToken);
        Task<WorkAssignment> UpdateAsync(WorkAssignment workAssignment, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
