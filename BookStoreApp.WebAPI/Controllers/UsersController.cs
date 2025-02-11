using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BookStoreApp.Business.DTOs;
using Microsoft.AspNetCore.Identity;
using BookStoreApp.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _environment;

        public UsersController(IUserService userService, ICacheService cacheService, IAuthService authService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _cacheService = cacheService;
            _authService = authService;
            _environment = environment;
        }

        public static User user = new();

        [HttpGet("loggedUser")]
        public async Task<IActionResult> GetUser()
        {
            // Get username from cookies
            if (!Request.Cookies.TryGetValue("username", out string? username) || string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User not logged in" });
            }

            // Get user information from Redis cache
            var cachedUser = await _cacheService.GetUserAsync(username);

            if (string.IsNullOrEmpty(cachedUser))
            {
                return NotFound(new { message = "User not found" });
            }

            // Deserialize user information
            var user = JsonSerializer.Deserialize<User>(cachedUser);

            return Ok(new { isLoggedIn = true, user });
        }
    }
}

