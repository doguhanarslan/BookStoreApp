using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;
using BookStoreApp.DataAccess.Abstract;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfCategoryDal:EfEntityRepositoryBase<Category,BookstoreContext>,ICategoryDal
    {
        private BookstoreContext _context;

        public EfCategoryDal(BookstoreContext context)
        {
            _context = context;
        }


        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
    }
}
