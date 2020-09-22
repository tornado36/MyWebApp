using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyWebAppCore.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext baseDbContext;
        protected DbSet<T> entitySet;
        private readonly ILogger _logger;

        public BaseRepository(DbContext dbContext, ILogger<BaseRepository<T>> logger)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            string sErr = "";
            try
            {
                _logger = logger;
                baseDbContext = dbContext;
                entitySet = dbContext.Set<T>();
            }
            catch
            {
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                sErr = Environment.NewLine + sErr;
                _logger.LogError(sErr);
            }
        }
        public virtual async Task<T[]> GetAllAsync()
        {
            T[] result = null;
            string sErr = "";

            try
            {
                result = await entitySet.ToArrayAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate)
        {
            IEnumerable<T> result = null;
            string sErr = "";

            try
            {
                result = await entitySet.ToArrayAsync();
                if (predicate != null)
                    result = result.Where(predicate);
            }
            catch
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> InsertAsync(T entity)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entity == null)
                {
                    // just return, not an error.
                    goto Get_Out;
                }
                try
                {
                    await entitySet.AddAsync(entity);
                    result = await baseDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    if (!entitySet.Contains(entity))
                    {
                        await entitySet.AddAsync(entity);
                    }
                    result = await baseDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> InsertBatchAsync(IEnumerable<T> entities)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entities == null || entities.Count() == 0)
                {
                    // just return, not an error.
                    goto Get_Out;
                }
                await entitySet.AddRangeAsync(entities);
                result = await baseDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entity == null)
                {
                    // just return, not an error.
                    goto Get_Out;
                }

                baseDbContext.Update(entity);
                result = await baseDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entity == null)
                {
                    // just return, not an error.
                    goto Get_Out;
                }
                baseDbContext.Remove(entity);
                result = await baseDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> UpdateBatchAsync(IEnumerable<T> entities)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entities == null || entities.Count() == 0)
                {
                    // just return, not an error.
                    goto Get_Out;
                }
                try
                {
                    baseDbContext.UpdateRange(entities);
                    result = await baseDbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    result = await baseDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<int> DeleteBatchAsync(IEnumerable<T> entities)
        {
            int result = 0;
            string sErr = "";
            try
            {
                if (entities == null || entities.Count() == 0)
                {
                    // just return, not an error.
                    goto Get_Out;
                }
                baseDbContext.RemoveRange(entities);
                result = await baseDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            T result = null;
            string sErr = "";

            try
            {
                if (predicate != null)
                    result = await entitySet.FirstOrDefaultAsync(predicate);
                else
                    result = await entitySet.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                _logger.LogError(sErr);
            }

            return result;
        }

        public virtual async Task<T> GetFirstOrDefaultAsync()
        {
            T result = null;
            string sErr = "";

            try
            {
                result = await entitySet.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual bool IfExist(Func<T, bool> predicate)
        {
            string sErr = String.Empty;
            bool result = false;
            try
            {
                result = entitySet.Where(predicate).Count() > 0;
            }
            catch (Exception ex)
            {
                sErr += "Fail to predicate if record exist.";
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                _logger.LogError(sErr);
            }
            return result;
        }

        public virtual async Task<string> DeleteAllAsync(Func<T, bool> predicate)
        {
            string sErr = string.Empty;
            IEnumerable<T> entities;
            int deleteCount = 0;
            int exitingCount = 0;
            try
            {

                entities = entitySet.Where(predicate);
                exitingCount = entities.Count();
                if (entities != null && exitingCount > 0)
                {
                    deleteCount = await DeleteBatchAsync(entities);
                    if (deleteCount != exitingCount)
                    {
                        sErr = $"Delete records failed: find {entities.Count()} record(s) but only delete {deleteCount}.";
                    }
                }
            }
            catch (Exception ex)
            {
                sErr += "Fail to delete records by condition.";
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return sErr;
        }

        public virtual async Task<string> DeleteAllAsync()
        {
            string sErr = string.Empty;
            int deleteCount = 0;
            try
            {
                if (entitySet != null && entitySet.Any())
                {
                    deleteCount = await DeleteBatchAsync(entitySet);
                }
            }
            catch (Exception ex)
            {
                sErr += "Fail to delete all records.";
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            if (sErr.Length > 0)
            {
                _logger.LogError(sErr);
            }
            return sErr;
        }
    }
}
