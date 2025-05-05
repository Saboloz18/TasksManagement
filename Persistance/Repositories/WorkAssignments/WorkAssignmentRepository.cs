using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Persistance.Repositories.WorkAssignments
{
    public class WorkAssignmentRepository : IWorkAssignmentRepository
    {
        private readonly TaskManagementDbContext _context;

        public WorkAssignmentRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<WorkAssignment> AddAsync(WorkAssignment workAssignment, CancellationToken cancellationToken)
        {
            _context.WorkAssignments.Add(workAssignment);
            await _context.SaveChangesAsync(cancellationToken);
            return workAssignment;
        }

        public async Task<WorkAssignment?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.WorkAssignments
                .Include(wa => wa.User)
                .FirstOrDefaultAsync(wa => wa.Id == id, cancellationToken);
        }

        public async Task<List<WorkAssignment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.WorkAssignments
                .Include(wa => wa.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<WorkAssignment>> GetByWorkIdAsync(int workId, CancellationToken cancellationToken)
        {
            return await _context.WorkAssignments
                .Include(wa => wa.User)
                .Where(wa => wa.WorkId == workId)
                .ToListAsync(cancellationToken);
        }

        public async Task<WorkAssignment> UpdateAsync(WorkAssignment workAssignment, CancellationToken cancellationToken)
        {
            _context.WorkAssignments.Update(workAssignment);
            await _context.SaveChangesAsync(cancellationToken);
            return workAssignment;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var workAssignment = await _context.WorkAssignments.FindAsync(new object[] { id }, cancellationToken);
            if (workAssignment != null)
            {
                _context.WorkAssignments.Remove(workAssignment);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
