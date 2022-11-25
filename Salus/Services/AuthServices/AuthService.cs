using Microsoft.Data.SqlClient;
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
        public void SendToken(Auth auth)
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
            string body = 
                $"<div style=\"background-color:#01a36269;\r\n" +
                $"            height: 100%;\r\n" +
                $"            scroll-behavior: smooth;\">\r\n" +
                $"    <div style=\"margin-left: 20%;\r\n" +
                $"                background-color: rgb(255, 255, 255);\r\n" +
                $"                width: 60%;\r\n" +
                $"                padding-bottom: 2vw;\">\r\n" +
                $"        <img style=\"width: 50%;\r\n" +
                $"                    margin-left: 25%;\"\r\n" +
                $"                    src='{imgUrl}' alt=\"Logo\">\r\n" +
                $"        <h1 style=\"text-align: center;\r\n" +
                $"                    font-size: 4vw;\r\n" +
                $"                    color:#01a362;\r\n" +
                $"                    font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;\">\r\n" +
                $"                    Click the button, </br>\r\n" +
                $"                    to verify your account!\r\n" +
                $"        </h1>\r\n" +
                $"        <div style=\"margin: 5vw;\">\r\n" +
                $"            <h2 style=\"font-size: 3.5vw;\r\n" +
                $"                        text-align: center;\">\r\n" +
                $"                        Hi {auth.username}!\r\n" +
                $"            </h2>\r\n" +
                $"            <p style=\"font-size: 2.5vw;\">\r\n" +
                $"                Thanks for using Salus - Healthy lifestyle!\r\n" +
                $"                Please click on the button below to confirm your email address.\r\n" +
                $"            </p>\r\n" +
                $"            <strong style=\"font-size: 2.5vw;\">After the verification process is complete:</strong>\r\n" +
                $"            <ul>\r\n" +
                $"                <li style=\"font-size: 2.5vw;\">\r\n" +
                $"                    Please set your personal information!\r\n" +
                $"                </li>\r\n" +
                $"                <li style=\"font-size: 2.5vw;\">\r\n" +
                $"                    Enjoy our app!\r\n" +
                $"                </li>\r\n" +
                $"            </ul>\r\n" +
                $"            <form name=\"myForm\" target=\"_blank\" action='{url}' method=\"post\">\r\n" +
                $"                <button style=\"font-family: Georgia, 'Times New Roman', Times, serif;\r\n" +
                $"                            align-items: center;\r\n" +
                $"                            background-image: linear-gradient(144deg,#01a362, #00cf7c 50%,#01a362);\r\n" +
                $"                            border: 0;\r\n" +
                $"                            border-radius: 20px;\r\n" +
                $"                            color: #ffffff;\r\n" +
                $"                            display: flex;\r\n" +
                $"                            font-size: 2.5vw;\r\n" +
                $"                            justify-content: center;\r\n" +
                $"                            padding: 3px;\r\n" +
                $"                            cursor: pointer;\r\n" +
                $"                            margin-top: 5vw;\r\n" +
                $"                            width: 80%;\r\n" +
                $"                            margin-left: 10%;\" type=\"submit\" name=\"submit_param\" value=\"submit_value\">\r\n" +
                $"                            <span class=\"text\"\r\n" +
                $"                                  style=\"background-color: #005734;\r\n" +
                $"                                  padding: 16px 24px;\r\n" +
                $"                                  border-radius: 20px;\r\n " +
                $"                                  width: 100%;\r\n" +
                $"                                  height: 3vw;\">\r\n" +
                $"                                  VERIFY\r\n" +
                $"                            </span>\r\n" +
                $"                </button>\r\n" +
                $"            </form>\r\n" +
                $"        </div>\r\n" +
                $"    </div>\r\n" +
                $"</div>";

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

            var send_mail = new MailMessage
            {
                Body = body,
                Subject = subject,
                From = new MailAddress(from),
                IsBodyHtml = true
            };

            send_mail.To.Add(new MailAddress(to));
            try
            {
                client.Send(send_mail);
            }
            catch
            {
                throw new Exception("Email address doesn't exist.");
            }
        }
        public string GetEmail()
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            return result;
        }

        public Auth NewAuth(AuthRegisterRequest request)
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

        public string CreateToken(Auth auth)
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

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
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
    }
}
