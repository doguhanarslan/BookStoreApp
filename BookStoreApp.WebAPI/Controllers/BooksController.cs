using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
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
        private ICacheService _cacheService;
        private readonly IUserService _userService;

        public BooksController(IBookService bookService, ICacheService cacheService, IUserService userService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("getBookById/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("GetBookByName/{title}")]
        public IActionResult GetBookByName(string title)
        {
            var book = _bookService.GetBookByName(title);
            return Ok(book);
        }

        [HttpPut]
        public Book AddToCart(Book book)
        {
            return _bookService.Add(book);
        }
    }
}
