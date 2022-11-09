using Authentication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
namespace Authentication.Services.AuthServices
{
    public interface IAuthService
    {
        public string GetEmail();
        Auth NewAuth(AuthRegisterRequest request);
        string CreateToken(Auth auth);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void SetTokenAndExpires(Auth auth);
        void UpdateAuthResetPasswordData(string password, Auth auth);
    }
}
