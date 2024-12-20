using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BookStoreApp.Business.DTOs;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        ICacheService _cacheService;

        public UsersController(IUserService userService, ICacheService cacheService)
        {
            _userService = userService;
            _cacheService = cacheService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userService.ValidateUserAsync(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Serialize user data and store it in Redis
            var userData = JsonSerializer.Serialize(user);
            await _cacheService.SetUserAsync(model.Username, userData);

            // Set the username cookie
            Response.Cookies.Append("username", model.Username, new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            return Ok(new { message = "Login successful" });
        }

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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                // Redis'teki kullanıcı bilgisini sil
                var key = $"user_{model.UserName}";
                await _cacheService.Clear(key);

                // Cookie'yi temizle
                Response.Cookies.Delete("username", new CookieOptions
                {
                    HttpOnly = false,
                    Path = "/", // Cookie oluşturulurken kullanılan path ile aynı olmalı
                });

                // Silindiğini kontrol edin
                var cachedUser = await _cacheService.GetUserAsync(model.UserName);
                if (!string.IsNullOrEmpty(cachedUser))
                {
                    return StatusCode(500, new { message = "Redis verisi temizlenemedi!" });
                }
            }

            return Ok(new { message = "Logout successful" });
        }
    }
}

