using BookStoreApp.Business.Abstract;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUser(string userName, string password)
        {
            try
            {
                var user = _userService.GetUser(userName, password);
                return Ok(user);

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
