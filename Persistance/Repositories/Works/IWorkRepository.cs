using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;

namespace TasksManagement.Persistance.Repositories.Works
{
    public interface IWorkRepository
    {
        Task<Work> AddAsync(Work work, CancellationToken cancellationToken);
        Task<Work?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Work>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<Work>> GetNonCompletedWorksAsync(CancellationToken cancellationToken);
        Task<Work> UpdateAsync(Work work, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken);
    }
}
