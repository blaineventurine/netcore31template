using System;
using System.Collections.Generic;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Persistence
{
    public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        public TContext Context { get; }

        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(T);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TContext, T>(Context);
            return (IRepository<T>)_repositories[type];
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
