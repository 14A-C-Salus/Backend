using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Salus.Services.AuthServices
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

        public void SetTokenAndExpires(Auth auth)
        {
            CheckAuthData(auth);

            auth.passwordResetToken = CreateRandomToken();
            auth.resetTokenExpires = DateTime.Now.AddDays(1);
        }

        public void UpdateAuthResetPasswordData(string password, Auth auth)
        {
            if (auth.resetTokenExpires == null || auth.passwordResetToken == null)
                throw new Exception("You need first use the 'forgoted-password' service!");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            auth.passwordHash = passwordHash;
            auth.passwordSalt = passwordSalt;
            auth.passwordResetToken = null;
            auth.resetTokenExpires = null;
        }
        public async Task<Auth> Register(AuthRegisterRequest request)
        {
            if (_dataContext.auths.Any(a => a.email == request.email))
                throw new Exception("Email already exists.");

            var auth = NewAuth(request);
            if (_configuration.GetSection("Host:Use").Value != "LocalDB")
                SendToken(auth);
            _dataContext.auths.Add(auth);
            await _dataContext.SaveChangesAsync();
            return auth;
        }
        public async Task<string> Login(AuthLoginRequest request)
        {
            var auth = await _dataContext.auths.FirstAsync(a => a.email == request.email);

            if (auth == null || !VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt))
                throw new Exception("Username or password is not correct!");

            if (auth.date == null)
                throw new Exception($"Not verified! Check your emails and verify your account!");

            string jwt = CreateToken(auth);
            return jwt;
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
            CheckPassword(password);

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private void CheckPassword(string password)
        {
            if (password.Length > 20 || password.Length < 8)
                throw new Exception("Invalid password!");
        }

        private void CheckAuthData(Auth auth)
        {
            if (auth == null)
                throw new Exception("Empty auth.");
            if (auth.username.Length < 8 || auth.username.Length > 20)
                throw new Exception("Invalid username.");
            if (auth.email.Length < 8 || auth.email.Length > 200)
                throw new Exception("Invalid email.");
        }
        private void CheckRegisterRequest(AuthRegisterRequest request)
        {
            if (request.email.Length < 8 || request.email.Length > 200 || !request.email.Contains("@") || !request.email.Contains("."))
                throw new Exception("Invalid email!");
            if (request.username.Length < 8 || request.username.Length > 20)
                throw new Exception("Invalid username!");
            if (request.password.Length < 8 || request.password.Length > 20)
                throw new Exception("Invalid password!");
            if (request.confirmPassword != request.password)
                throw new Exception("Invalid confirm password!");
        }

        private Auth NewAuth(AuthRegisterRequest request)
        {
            CheckRegisterRequest(request);
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
        private void SendToken(Auth auth)
        {
            var template = new EmailBodyTemplate();

            string to = auth.email;
            string from = _configuration.GetSection("MailSettings:From").Value;
            string password = _configuration.GetSection("MailSettings:Password").Value;
            string subject = "Verify your account";

            string url = $"http://salus.bsite.net/api/Auth/verify?token={auth.verificationToken}";
            string imgUrl = "https://i.ibb.co/Dg5h4zK/logo.png";

            if (_configuration.GetSection("Host:Use").Value == "LocalDB")
                url = $"https://localhost:7138/api/Auth/verify?token={auth.verificationToken}";
            else if (_configuration.GetSection("Host:Use").Value == "MyAspDB")
                url = $"http://anoblade-001-site1.atempurl.com/api/Auth/verify?token={auth.verificationToken}";

            string body = template.EmailBody(imgUrl, auth.username, url);

            SmtpClient client = new SmtpClient
            {
                Port = int.Parse(_configuration.GetSection("MailSettings:Port").Value),
                Host = _configuration.GetSection("MailSettings:Host").Value,
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)
            };

            var sendMail = new MailMessage
            {
                Body = body,
                Subject = subject,
                From = new MailAddress(from),
                IsBodyHtml = true
            };

            sendMail.To.Add(new MailAddress(to));

            try
            {
                client.Send(sendMail);
            }
            catch
            {
                throw new Exception("Email address doesn't exist.");
            }
        }
        private string CreateToken(Auth auth)
        {
            CheckAuthData(auth);
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedPasswordHash.SequenceEqual(passwordHash);
            }
        }
    }
}
