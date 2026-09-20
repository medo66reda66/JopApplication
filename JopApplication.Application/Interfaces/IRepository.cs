using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
          Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        
         void Update(T entity) ;

         void Delete(T entity) ;
          Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? exception = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default);


          Task<T?> GetByIdAsync(
            Expression<Func<T, bool>> exception,
            Expression<Func<T, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default);

          Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
       
    }
}

