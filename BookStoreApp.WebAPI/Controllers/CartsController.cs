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
            string cartSessionId = HttpContext.Session.GetString("CartSessionId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartSessionId", cartSessionId);
            try
            {
                _cartService.AddToCart(bookId, cartSessionId);
                return Ok(new { Message = "Book added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("sessionId")]
        public IActionResult GetCartSessionId()
        {
            string cartSessionId = HttpContext.Session.GetString("CartSessionId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartSessionId", cartSessionId);
            return Ok(new { CartSessionId = cartSessionId });
        }

        [HttpGet("items")]
        public IActionResult GetCartDetails(string sessionId)
        {
            var cartItems = _cartService.GetCartItemsForSession(sessionId);
            return Ok(cartItems);
        }


        [HttpGet("getCartById")]
        public IActionResult GetBookCartById(int cartId)
        {
            var cart = _cartService.GetBookCartById(cartId);

            return Ok(cart);
        }

        [HttpGet("GetTotalPrice")]
        public IActionResult GetTotalPrice(string sessionId)
        {
            var totalPrice = _cartService.GetTotalPrice(sessionId);
            return Ok(totalPrice);
        }
    }
}
