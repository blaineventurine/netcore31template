using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Persistence
{
    public sealed class Repository<TContext, TEntity> : IRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly bool _isAuditable;

        public Repository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _isAuditable = typeof(TEntity).GetInterfaces().Any(i => i.Name.Equals(nameof(IAuditable)));
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate, bool disableTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking(); ;
        }

        public async Task<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity>> Add(TEntity entity)
        {
            if (_isAuditable && entity is IAuditable auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            return await _dbSet.AddAsync(entity);
        }

        public Task Add(params TEntity[] entities)
        {
            if (_isAuditable && entities is IAuditable[] auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            return _dbSet.AddRangeAsync(entities); ;
        }

        public Task Add(IEnumerable<TEntity> entities)
        {
            if (_isAuditable && entities is IEnumerable<TEntity> auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            return _dbSet.AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            var existing = _dbSet.Find(entity);
            if (existing != null) _dbSet.Remove(existing);
        }

        public void Delete(object id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public void Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            if (_isAuditable && entity is IAuditable auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            _dbSet.Update(entity);
        }

        public void Update(params TEntity[] entities)
        {
            if (_isAuditable && entities is IAuditable[] auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            if (_isAuditable && entities is IEnumerable<TEntity> auditableEntity)
                UpdateTrackingColumns(ref auditableEntity);

            _dbSet.UpdateRange(entities);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        private void UpdateTrackingColumns(ref IAuditable[] entities, bool overrideAuditable = false)
        {

            foreach (var entity in entities)
            {
                if (_isAuditable && entity is IAuditable auditableEntity)
                    UpdateTrackingColumns(ref auditableEntity, overrideAuditable);
            }
        }

        private void UpdateTrackingColumns(ref IEnumerable<TEntity> entities, bool overrideAuditable = false)
        {
            foreach (var entity in entities)
            {
                if (_isAuditable && entity is IAuditable auditableEntity)
                    UpdateTrackingColumns(ref auditableEntity, overrideAuditable);
            }
        }

        private void UpdateTrackingColumns(ref IAuditable entity, bool overrideAuditable = false)
        {
            if (!overrideAuditable && entity.CreatedDate == default)
            {
                entity.CreatedDate = DateTime.Now.ToUniversalTime();
                //entity.CreatedBy = _currentUser.Email;
                //entity.CreatedById = _currentUser.PortalUserId;
            }

            //entity.LastUpdateBy = _currentUser.Email;
            //entity.LastUpdateById = _currentUser.PortalUserId;
            if (!overrideAuditable)
                entity.LastUpdatedDate = DateTime.Now.ToUniversalTime();
        }

    }
}
