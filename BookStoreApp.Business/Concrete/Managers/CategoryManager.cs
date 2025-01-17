using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class CategoryManager:ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly ICacheService _cacheService;

        public CategoryManager(ICacheService cacheService, ICategoryDal categoryDal)
        {
            _cacheService = cacheService;
            _categoryDal = categoryDal;
        }

        public List<Category> GetAll()
        {
           return _cacheService.GetOrAdd($"categories", () => _categoryDal.GetAll());
        }
    }
}
