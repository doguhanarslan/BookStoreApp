using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BookStoreApp.Business.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        ICacheService _cacheService;
        private readonly IWebHostEnvironment _environment;
        public UsersController(IUserService userService, ICacheService cacheService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _cacheService = cacheService;
            _environment = environment;
        }

        public static User user = new();
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var hashedPw = new PasswordHasher<User>().HashPassword(UsersController.user, model.Password);





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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            // Profile image handling
            string profileImagePath = null;
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                profileImagePath = $"/uploads/{uniqueFileName}";
            }

            var user = new User
            {
                UserName = model.UserName,
                Password = model.Password, // Note: Password should be hashed!
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImage = profileImagePath
            };

            var result = await _userService.AddUserAsync(user);
            if (result == null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            return Ok(new { message = "User registered successfully" });
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

