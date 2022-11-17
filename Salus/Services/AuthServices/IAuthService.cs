namespace Salus.Services.AuthServices
{
    public interface IAuthService
    {
        public string GetEmail();
        Auth NewAuth(AuthRegisterRequest request);
        string CreateToken(Auth auth);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void SetTokenAndExpires(Auth auth);
        void UpdateAuthResetPasswordData(string password, Auth auth);
        void SendToken(Auth auth);
    }
}
