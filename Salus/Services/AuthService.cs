using Microsoft.IdentityModel.Tokens;
using Salus.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Salus.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly GenericService<Auth> _genericServices;
        public IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext dataContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _genericServices = new(dataContext, httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        //public methods
        public Auth? GetAuth(int authId)
        {
            var auth = _genericServices.Read(authId);
            if (auth == null)
                throw new EAuthNotFound();
            return auth;
        }
        public UserProfile? GetUserProfile(int authId)
        {
            var auth = _genericServices.Read(authId);
            if (auth == null)
                throw new EAuthNotFound();
            var userProfile = _dataContext.Set<UserProfile>().SingleOrDefault(u => u.auth == auth);
            if (userProfile == null)
                throw new EUserProfileNotFound();
            return userProfile;
        }

        public Auth Register(AuthRegisterRequest request)
        {
            if (_dataContext.Set<Auth>().Any(a => a.email == request.email))
                throw new EEmailAlreadyExist();
            var auth = NewAuth(request);
#if RELEASE
            SendToken(auth);
#endif
            _genericServices.Create(auth);
            return auth;
        }
        public async Task<string> Login(AuthLoginRequest request)
        {
            var auth = await _dataContext.Set<Auth>().FirstOrDefaultAsync(a => a.email == request.email);

            if (auth == null || !VerifyPasswordHash(request.password, auth.passwordHash, auth.passwordSalt))
                throw new EUsernamePasswordIncorrect();

            if (auth.date == null)
                throw new ENotVerified();

            string jwt = CreateToken(auth);
            return jwt;
        }

        public async Task<Auth> Verify(string token)
        {
            var auth = await _dataContext.Set<Auth>().FirstOrDefaultAsync(a => a.verificationToken == token);
            if (auth == null)
                throw new EInvalidToken();

            auth.date = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return auth;
        }

        public async Task<Auth> ForgotPassword(string email)
        {
            var auth = await _dataContext.Set<Auth>().FirstOrDefaultAsync(a => a.email == email);
            if (auth == null)
                throw new EUserNotFound();

            SetTokenAndExpires(auth);

            await _dataContext.SaveChangesAsync();
            return auth;
        }

        public async Task<Auth> ResetPassword(AuthResetPasswordRequest request)
        {
            var auth = await _dataContext.Set<Auth>().FirstOrDefaultAsync(a => a.passwordResetToken == request.token);
            if (auth == null || auth.resetTokenExpires < DateTime.Now)
                throw new EInvalidToken();

            UpdateAuthResetPasswordData(request.password, auth);

            await _dataContext.SaveChangesAsync();
            return auth;
        }

        //private methods
        private string CreateRandomToken()
        {
            var randomToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
            while (_dataContext.Set<Auth>().Any(a => a.verificationToken == randomToken) && _dataContext.Set<Auth>().Any(a => a.passwordResetToken == randomToken))
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
                throw new EInvalidPassword();
        }
        private void CheckAuthData(Auth auth)
        {
            if (auth == null)
                throw new EEmptyToken();
            if (auth.username.Length < 8 || auth.username.Length > 20)
                throw new EInvalidUsername();
            if (auth.email.Length < 8 || auth.email.Length > 200)
                throw new EInvalidEmail();
        }
        private void CheckRegisterRequest(AuthRegisterRequest request)
        {
            if (request.email.Length < 8 || request.email.Length > 200 || !request.email.Contains("@") || !request.email.Contains("."))
                throw new EInvalidEmail();
            if (request.username.Length < 8 || request.username.Length > 20)
                throw new EInvalidUsername();
            if (request.password.Length < 8 || request.password.Length > 20)
                throw new EInvalidPassword();
            if (request.confirmPassword != request.password)
                throw new EInvalidConfirmPassword();
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

            string body = File.ReadAllText("./Templates/EmailBody.html").Replace("IMAGEURL", imgUrl).Replace("USERNAME", auth.username).Replace("URL", url);

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
                throw new EEmailNotFound();
            }
        }
        private string CreateToken(Auth auth)
        {
            CheckAuthData(auth);
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", auth.id.ToString()),
                new Claim(ClaimTypes.Role, auth.isAdmin ? "Admin" : "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
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
        private void SetTokenAndExpires(Auth auth)
        {
            CheckAuthData(auth);

            auth.passwordResetToken = CreateRandomToken();
            auth.resetTokenExpires = DateTime.Now.AddDays(1);
        }
        private void UpdateAuthResetPasswordData(string password, Auth auth)
        {
            if (auth.resetTokenExpires == null || auth.passwordResetToken == null)
                throw new EPasswordResetTokenExpired();

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            auth.passwordHash = passwordHash;
            auth.passwordSalt = passwordSalt;
            auth.passwordResetToken = null;
            auth.resetTokenExpires = null;
        }
    }
}
