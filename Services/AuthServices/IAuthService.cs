using ChatApp.Api.DTOs;

namespace ChatApp.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(AuthRequest authRequest);

        Task<AuthResult> LoginAsync(AuthRequest request);
    }
}
