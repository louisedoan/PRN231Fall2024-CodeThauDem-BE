using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Repositories.Repositories.BaseRepository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<TEntity> _dbSet;

        private bool _disposed = false;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "")
        {
            IQueryable<TEntity> queryable = _dbSet;
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            string[] array = includeProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string navigationPropertyPath in array)
            {
                queryable = queryable.Include(navigationPropertyPath);
            }

            if (orderBy != null)
            {
                return orderBy(queryable).AsQueryable();
            }

            return queryable.AsQueryable();
        }

        public void UpdateRange(List<TEntity> entity)
        {
            _dbSet.UpdateRange(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public TEntity Create(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity FirstOrDefault()
        {
            return _dbSet.FirstOrDefault();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            return _dbSet.FirstOrDefaultAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public TEntity Get<TKey>(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<TEntity> Get()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<TEntity> GetAsync<TKey>(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public TEntity Update(TEntity entity)
        {
            return _dbSet.Update(entity).Entity;
        }
    }
}
