using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.DTOs;
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
        public IActionResult AddReview([FromBody] AddReviewDto dto)
        {
            try
            {
                _reviewService.AddReview(dto.BookId,dto.UserId,dto.UserName, dto.ReviewText, dto.Rating);
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
