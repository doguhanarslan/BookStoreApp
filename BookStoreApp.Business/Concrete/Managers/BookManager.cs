using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class BookManager : IBookService
    {
        private readonly IBookDal _bookDal;
        private readonly ICacheService _cacheService;
        

        public BookManager(IBookDal bookDal, ICacheService cacheService)
        {
            _bookDal = bookDal;
            _cacheService = cacheService;
            
        }

        public List<BookDetails> GetAllBooks()
        {
            var books = GetAllBooksFromCache();
            return books;
        }

        public List<BookDetails> GetAllBooksFromCache()
        {
            return _cacheService.GetOrAdd("all_books", () => _bookDal.GetAllBooks());
        }

        public Book Add(Book book)
        {
            var addedBook = _bookDal.Add(book);
            var bookDetails = _bookDal.GetBookById(addedBook.Id);
            
            return addedBook;
        }

        public Book Update(Book book)
        {
            var updatedBook = _bookDal.Update(book);
            var bookDetails = _bookDal.GetBookById(updatedBook.Id);
           
            return updatedBook;
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
