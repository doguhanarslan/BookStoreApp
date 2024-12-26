using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private IUserService _userService;
        private ICacheService _cacheService;

        public CartsController(ICartService cartService, ICacheService cacheService, IUserService userService)
        {
            _cartService = cartService;
            _cacheService = cacheService;
            _userService = userService;
        }

        [HttpPost("add")]
        public IActionResult AddToCart(int bookId,int userId,int quantity)
        {
            
            try
            {
                _cartService.AddToCart(bookId, userId,quantity);
                return Ok(new { Message = "Book added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //[HttpGet("sessionId")]
        //public IActionResult GetCartSessionId()
        //{
        //    string cartSessionId = HttpContext.Session.GetString("CartSessionId") ?? Guid.NewGuid().ToString();
        //    HttpContext.Session.SetString("CartSessionId", cartSessionId);
        //    return Ok(new { CartSessionId = cartSessionId });
        //}

        [HttpGet("items")]
        public IActionResult GetCartDetails(int userId)
        {
            var cartItems = _cartService.GetCartItemsForUser(userId);
            return Ok(cartItems);
        }

        [HttpDelete("removeFromCart")]
        public void DeleteCart(int bookId,int userId)
        {
            _cartService.RemoveFromCart(bookId, userId);
        }

        [HttpGet("getCartById")]
        public IActionResult GetBookCartById(int cartId)
        {
            var cart = _cartService.GetBookCartById(cartId);

            return Ok(cart);
        }

        [HttpGet("GetTotalPrice")]
        public IActionResult GetTotalPrice(int userId)
        {
            var totalPrice = _cartService.GetTotalPrice(userId);
            return Ok(totalPrice);
        }
    }
}
