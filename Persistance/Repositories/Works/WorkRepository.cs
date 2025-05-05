using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;

namespace TasksManagement.Persistance.Repositories.Works
{
    public class WorkRepository : IWorkRepository
    {
        private readonly TaskManagementDbContext _context;

        public WorkRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Work> AddAsync(Work work, CancellationToken cancellationToken)
        {
            _context.Works.Add(work);
            await _context.SaveChangesAsync(cancellationToken);
            return work;
        }

        public async Task<Work?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Works
                .Include(w => w.AssignmentHistory)
                .ThenInclude(wa => wa.User)
                .Include(w => w.CurrentUser)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<List<Work>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Works
                .Include(w => w.AssignmentHistory)
                .ThenInclude(wa => wa.User)
                .Include(w => w.CurrentUser)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Work>> GetNonCompletedWorksAsync(CancellationToken cancellationToken)
        {
            return await _context.Works
                .Include(w => w.AssignmentHistory)
                .ThenInclude(wa => wa.User)
                .Where(w => w.State != WorkState.Completed)
                .ToListAsync(cancellationToken);
        }

        public async Task<Work> UpdateAsync(Work work, CancellationToken cancellationToken)
        {
            _context.Works.Update(work);
            await _context.SaveChangesAsync(cancellationToken);
            return work;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var work = await _context.Works.FindAsync(new object[] { id }, cancellationToken);
            if (work != null)
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken)
        {
            return await _context.Works.AnyAsync(w => w.Title == title, cancellationToken);
        }
    }
}
