using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CacheAspect.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);
        
        void Insert(TEntity entity);
        
        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
