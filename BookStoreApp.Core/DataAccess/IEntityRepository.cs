using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Core.DataAccess
{
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        List<T> GetAll();

        T Get(Expression<Func<T, bool>> filter = null);

        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
