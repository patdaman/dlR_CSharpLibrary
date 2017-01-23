using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ConcreteRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private const string ErrorMessage_DbContextNull = "DbContext null. Did you initialize your repository variable?";
        DbContext dbContext;

        public ConcreteRepository(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<TEntity> Entities
        {
            get
            {
                if (dbContext == null)
                    throw new NullReferenceException(ErrorMessage_DbContextNull);
                return dbContext.Set<TEntity>();
            }
        }


        void IRepository<TEntity>.Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (dbContext == null)
                throw new NullReferenceException(ErrorMessage_DbContextNull);

            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Insert a collection of new entities. These are inserted in a transactional manner.
        /// </summary>
        /// <param name="entity"></param>
        void IRepository<TEntity>.Insert(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException("entity");

            if (entityList.Count() == 0)
                return; 

            if (dbContext == null)
                throw new NullReferenceException(ErrorMessage_DbContextNull);

            foreach( TEntity entity in entityList)
                dbContext.Set<TEntity>().Add(entity);

            dbContext.SaveChanges();
        }


        void IRepository<TEntity>.Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (dbContext == null)
                throw new NullReferenceException(ErrorMessage_DbContextNull);


            dbContext.SaveChanges();            
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }


        public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (dbContext == null)
                throw new NullReferenceException(ConcreteRepository<TEntity>.ErrorMessage_DbContextNull);

            IQueryable<TEntity> query = dbContext.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        /// <summary>
        /// Refreshes the state of the given entity from the repository.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Reload(TEntity entity)
        {
            if (dbContext == null)
                throw new NullReferenceException(ErrorMessage_DbContextNull);

            dbContext.Entry(entity).Reload();
        }


        public void Delete(TEntity entity) 
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
