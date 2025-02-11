using BookStoreApp.Entities.Concrete;
using BookStoreApp.Entities.DTOs;

namespace BookStoreApp.WebAPI.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);

        Task<User?> ValidateRefreshTokenAsync(Guid userId,string refreshToken);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

        Task<User?> GetLoggedInUserAsync();
        Task<bool> LogoutAsync(Guid userId, string refreshToken);

    }
}
