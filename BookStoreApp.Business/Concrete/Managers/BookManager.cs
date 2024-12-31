using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public BookDetails GetBookByName(string name)
        {
            string n = FormatBookTitle(name);
            return _bookDal.GetBookByName(n);
        }

        public BookDetails GetBookById(int id)
        {
            return _bookDal.GetBookById(id);
        }

        public string FormatBookTitle(string bookTitle)
        {
            var normalizedBookTitle = bookTitle
                .Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i")
                .Replace("ö", "o").Replace("ş", "s").Replace("ü", "u")
                .ToLower()
                .Replace(" ", "-");
            return normalizedBookTitle;
        }

        public void DeleteBookById()
        {
            throw new NotImplementedException();
        }
    }
}
