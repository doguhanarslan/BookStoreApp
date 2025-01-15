using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

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