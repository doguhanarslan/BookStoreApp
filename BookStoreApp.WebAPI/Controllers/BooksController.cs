﻿using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using BookStoreApp.Business.Abstract;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            //string cacheKey = "all_books";
            //string cachedBooks = await _redis.StringGetAsync(cacheKey);

            //if (!string.IsNullOrEmpty(cachedBooks))
            //{
            //    _logger.LogInformation("Fetching books from cache.");
            //    var books = JsonSerializer.Deserialize<List<BookDetails>>(cachedBooks);
            //    return Ok(books);
            //}

            //_logger.LogInformation("Fetching books from database.");
            //var booksFromDb = _bookService.GetAllBooks();
            //var serializedBooks = JsonSerializer.Serialize(booksFromDb);
            //await _redis.StringSetAsync(cacheKey, serializedBooks, TimeSpan.FromMinutes(30));
            var books = _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpPut]
        public Book AddToCart(Book book)
        {
            return _bookService.Add(book);
        }
    }
}
