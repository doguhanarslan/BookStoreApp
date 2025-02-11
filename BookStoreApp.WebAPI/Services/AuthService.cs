using BookStoreApp.DataAccess.Concrete.EntityFrameworkCore;
using BookStoreApp.Entities.Concrete;
using BookStoreApp.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookStoreApp.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly BookstoreContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(BookstoreContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
                return null;

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                ProfileImage = request.ProfileImage
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = 2 }); // User role
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user is null || VerifyPassword(user, request.Password) == false)
                return null;

            var tokenResponse = await CreateTokenResponse(user, isInitialLogin: true);

            return tokenResponse;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            var tokenResponse = await CreateTokenResponse(user, isInitialLogin: false);

            return tokenResponse;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user, bool isInitialLogin)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user, isInitialLogin)
            };
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user, bool isInitialLogin)
        {
            user.RefreshToken = GenerateRefreshToken();

            // Refresh Token süresini SADECE ilk login'de ayarla
            if (isInitialLogin)
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
            return user.RefreshToken;
        }

        private bool VerifyPassword(User user, string password)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(
                user,
                user.PasswordHash,
                password) == PasswordVerificationResult.Success;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user is null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetLoggedInUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return null;
            }

            var userId = Guid.Parse(userIdClaim);
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> LogoutAsync(Guid userId, string refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(userId, refreshToken);
            if (user == null)
            {
                return false;
            }

            // Remove the refresh token from the database
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
