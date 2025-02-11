using BookStoreApp.Entities.Concrete;
using BookStoreApp.Entities.DTOs;
using BookStoreApp.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {
            var user = await _authService.RegisterAsync(request);

            if (user is null)
                return BadRequest("Username already exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
                return BadRequest("Invalid username or password");

            // Set a cookie with the username
            Response.Cookies.Append("username", request.UserName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);

            if (result is null)
                return Unauthorized("Invalid refresh token or token expired");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("authenticated-endpoint")]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated.");
        }


        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<User?>> GetLoggedInUser()
        {
            var user = await _authService.GetLoggedInUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.LogoutAsync(request.UserId, request.RefreshToken);
            if (!result)
            {
                return BadRequest(new { message = "Logout failed" });
            }

            return Ok(new { message = "Logout successful" });
        }
    }
}