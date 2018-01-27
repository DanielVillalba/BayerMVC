using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PSD.Repository.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class //, IEntity
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Remove(int id);
        void Remove(TEntity entity);
    }
}