using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.Aspects.Postsharp.CacheAspects;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;
using PostSharp.Aspects;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class BookManager:IBookService
    {
        private IBookDal _bookDal;
        private ICacheService _cacheService;
        public BookManager(IBookDal bookDal, ICacheService cacheService)
        {
            _bookDal = bookDal;
            _cacheService = cacheService;
        }
        public List<BookDetails> GetAllBooks()
        {
            return GetAllBooksFromCache();
        }

        public List<BookDetails> GetAllBooksFromCache()
        {
            return _cacheService.GetOrAdd("all_books", () => _bookDal.GetAllBooks() );
        }

        public Book Add(Book book)
        {
            return _bookDal.Add(book);
        }

        public Book Update(Book book)
        {
            return _bookDal.Update(book);
        }

        public Book GetBookByName(string name)
        {
            throw new NotImplementedException();
        }

        public Book GetBookById(int id)
        {
            return _bookDal.Get(b => b.Id == id);
        }

        public void DeleteBookById()
        {
            throw new NotImplementedException();
        }
    }
}
