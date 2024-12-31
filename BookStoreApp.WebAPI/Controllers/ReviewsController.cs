using BookStoreApp.Business.Abstract;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("/addReview")]
        public IActionResult AddReview(int bookId, int userId, string reviewText, int rating)
        {
            try
            {
                _reviewService.AddReview(bookId, userId, reviewText, rating);
                return Ok(new { Message = "Review added." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("/getReviewsByBookId")]
        public IActionResult GetReviewsByBookId(int bookId)
        {
            try
            {
                var reviews = _reviewService.GetReviewsByBookId(bookId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
