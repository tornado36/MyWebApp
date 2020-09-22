using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyWebAppCore.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetFirstOrDefaultAsync();
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T[]> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate);
        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<int> InsertBatchAsync(IEnumerable<T> entities);
        Task<int> UpdateBatchAsync(IEnumerable<T> entities);
        Task<int> DeleteBatchAsync(IEnumerable<T> entities);

    }
}
