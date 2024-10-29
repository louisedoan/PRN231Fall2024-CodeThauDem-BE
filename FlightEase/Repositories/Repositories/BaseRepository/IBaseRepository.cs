using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "");

        int Count();

        int GetMaxId(Expression<Func<TEntity, int>> predicate);

        TEntity Get<TKey>(TKey id);

        Task<TEntity> GetAsync<TKey>(TKey id);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault();

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync();

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity Create(TEntity entity);

        Task CreateAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void UpdateRange(List<TEntity> entity);

        void Delete(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Save();

        Task SaveAsync();

        void Dispose();

    }
}
