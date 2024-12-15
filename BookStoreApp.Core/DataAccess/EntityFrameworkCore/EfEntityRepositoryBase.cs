using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Core.DataAccess.EntityFrameworkCore
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()
    {

        public List<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        public TEntity? Get(Expression<Func<TEntity, bool>>? filter = null)
        {
            using (var context = new TContext())
            {
                if (filter != null)
                {
                    return context.Set<TEntity>().SingleOrDefault(filter);
                }
                return context.Set<TEntity>().SingleOrDefault();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
                return entity;
            }
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {

                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
                return entity;
            }

        }
    }
}
