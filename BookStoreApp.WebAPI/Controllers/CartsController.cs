using BookStoreApp.Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public IActionResult AddToCart(int bookId)
        {
            try
            {
                _cartService.AddToCart(bookId);
                return Ok(new { Message = "Book added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("items")] 
        public IActionResult GetCartItems()
        {
            var items = _cartService.GetCartItems();
            return Ok(items);
        }


        [HttpGet("getCartById")]
        public IActionResult GetBookCartById(int cartId)
        {
            var cart = _cartService.GetBookCartById(cartId);

            return Ok(cart);
        }

        [HttpGet("GetTotalPrice")]
        public IActionResult GetTotalPrice()
        {
            var totalPrice = _cartService.GetTotalPrice();
            return Ok(totalPrice);
        }
    }
}
