using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectCaching.Interface
{
    public interface IEfRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
    }
}
