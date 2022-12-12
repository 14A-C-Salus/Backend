namespace Salus.Services.AuthServices
{
    public interface IAuthService
    {
        public string GetEmail();
        void SetTokenAndExpires(Auth auth);
        void UpdateAuthResetPasswordData(string password, Auth auth);
        Task<Auth> Register(AuthRegisterRequest request);
        Task<string> Login(AuthLoginRequest request);
    }
}
