using Microsoft.EntityFrameworkCore;
using ObjectCaching.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace ObjectCaching.Interface
{
    public class EfRepository<T> : IEfRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }
    }
}
