using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Authentication.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthService(IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        //public methods
        public string GetEmail()
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            return result;
        }

        public Auth NewAuth(AuthRegisterRequest request)
        {
            CreatePasswordHash(request.password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

            var auth = new Auth
            {
                username = request.username,
                email = request.email,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt,
                verificationToken = CreateRandomToken()
            };

            return auth;
        }

        public string CreateToken(Auth auth)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, auth.username),
                new Claim(ClaimTypes.Email, auth.email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Keys:JwtKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedPasswordHash.SequenceEqual(passwordHash);
            }
        }

        public void SetTokenAndExpires(Auth auth)
        {
            auth.passwordResetToken = CreateRandomToken();
            auth.resetTokenExpires = DateTime.Now.AddDays(1);
        }

        public void UpdateAuthResetPasswordData(string password, Auth auth)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            auth.passwordHash = passwordHash;
            auth.passwordSalt = passwordSalt;
            auth.passwordResetToken = null;
            auth.resetTokenExpires = null;
        }

        //private methods
        private string CreateRandomToken()
        {
            var randomToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
            while (_dataContext.auths.Any(a => a.verificationToken == randomToken) && _dataContext.auths.Any(a => a.passwordResetToken == randomToken))
            {
                randomToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
            }
            return randomToken;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
