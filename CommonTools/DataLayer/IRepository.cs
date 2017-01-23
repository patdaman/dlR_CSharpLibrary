using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;

        /// <summary>
        /// Insert a new entity.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert a collection of new entities. These are inserted in a transactional manner.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(IEnumerable<TEntity> entity);

        void Update(TEntity entity);

        /// <summary>
        /// Refreshes the state of the given entity from the repository.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Reload(TEntity entity);

        void Delete(TEntity entity);

    }
}
