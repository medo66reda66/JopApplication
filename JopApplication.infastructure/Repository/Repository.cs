using JopApplication.Application.Interfaces;
using JopApplication.infrastructure.Persistenc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBcontext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDBcontext context)
        {
            _context = context;
            _dbSet = _context.Set <T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T,bool>>? exception = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (exception != null)
            {
                query = query.Where(exception);
            }
            if (include != null)
            {
                foreach (var includeProperty in include)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(
            Expression<Func<T, bool>> exception,
            Expression<Func<T, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default)
        {
            return (await GetAllAsync(exception, include, tracking, cancellationToken)).FirstOrDefault();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }catch (Exception ex)
            {
                Debug.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }
    }
}
