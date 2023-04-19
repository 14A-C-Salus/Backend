
namespace Salus.Services.AuthServices
{
    public interface IAuthService
    {
        Auth? GetAuth(int authId);
        UserProfile? GetUserProfile(int authId);
        Auth Register(AuthRegisterRequest request);
        Task<string> Login(AuthLoginRequest request);
        Task<Auth> Verify(string token);
        Task<Auth> ForgotPassword(string email);
        Task<Auth> ResetPassword(AuthResetPasswordRequest request);
    }
}
