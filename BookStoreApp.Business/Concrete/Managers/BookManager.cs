using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.DTOs;
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
        private readonly IElasticsearchService _elasticsearchService;

        public BookManager(IBookDal bookDal, ICacheService cacheService, IElasticsearchService elasticsearchService)
        {
            _bookDal = bookDal;
            _cacheService = cacheService;
            _elasticsearchService = elasticsearchService;
        }

        public List<BookDetails> GetAllBooks()
        {
            var books = GetAllBooksFromCache();
            IndexBooksInElasticsearch(books).Wait();
            return books;
        }

        public List<BookDetails> GetAllBooksFromCache()
        {
            return _cacheService.GetOrAdd("all_books", () => _bookDal.GetAllBooks());
        }

        public Book Add(Book book)
        {
            var addedBook = _bookDal.Add(book);
            return addedBook;
        }

        public Book Update(Book book)
        {
            var updatedBook = _bookDal.Update(book);
            return updatedBook;
        }

        public List<BookDetails> GetBooksByCategoryId(int categoryId)
        {
            return _cacheService.GetOrAdd($"categories_{categoryId}", () => _bookDal.GetBookByCategory(categoryId));
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

        private async Task IndexBooksInElasticsearch(List<BookDetails> books)
        {
            foreach (var book in books)
            {
                await _elasticsearchService.IndexBookAsync(new BookDetails { BookId = book.BookId, CategoryName = book.CategoryName, BookReviews = book.BookReviews, BookImage = book.BookImage, BookPrice = book.BookPrice, BookRate = book.BookRate, AuthorName = book.AuthorName, BookDescription = book.BookDescription, BookTitle = book.BookTitle });
            }
        }
    }
}
