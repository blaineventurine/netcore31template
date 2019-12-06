using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Service.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IEntity
    {
        Task<T> GetSingle(Expression<Func<T, bool>> predicate = null, bool disableTracking = true);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>> predicate, bool disableTracking = false);
        IQueryable<T> GetAll();
        Task<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>> Add(T entity);
        Task Add(params T[] entities);
        Task Add(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
    }
}
