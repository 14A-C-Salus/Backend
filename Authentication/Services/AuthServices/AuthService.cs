using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

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
        public void SendToken(Auth auth)
        {
            string to = auth.email;
            string from = _configuration.GetSection("MailSettings:From").Value;
            string password = _configuration.GetSection("MailSettings:Password").Value;
            string subject = "Verify your account";
            string url = $"https://salus.bsite.net/api/Auth/verify?token={auth.verificationToken}";
            if (_configuration.GetSection("Host:IsLocalHost").Value == "Yes")
                url = $"https://localhost:7138/api/Auth/verify?token={auth.verificationToken}";
            string body = $"<!DOCTYPE html>\r\n" +
                $"<html lang=\"en\">\r\n" +
                $"<head>\r\n" +
                $"    <meta charset=\"UTF-8\">\r\n" +
                $"    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n" +
                $"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n" +
                $"    <title>Verify</title>\r\n" +
                $"</head>\r\n<style>\r\n" +
                $"    body{{\r\n" +
                $"        background-color:#01a36269;\r\n" +
                $"        height: 100vw;\r\n" +
                $"        scroll-behavior: smooth;\r\n" +
                $"    }}\r\n" +
                $"    #main{{\r\n" +
                $"        margin-left: 20%;\r\n" +
                $"        background-color: rgb(255, 255, 255);\r\n" +
                $"        width: 60%;\r\n" +
                $"        padding-bottom: 2vw;\r\n" +
                $"    }}\r\n" +
                $"    #logo{{\r\n" +
                $"        width: 30vw;\r\n" +
                $"        margin-left: 15vw;\r\n" +
                $"    }}\r\n" +
                $"    #click{{\r\n " +
                $"       text-align: center;\r\n" +
                $"        font-size: 4vw;\r\n" +
                $"        color:#01a362;\r\n " +
                $"       font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;\r\n" +
                $"    }}\r\n" +
                $"    \r\n" +
                $"#button {{\r\n" +
                $"    font-family: Georgia, 'Times New Roman', Times, serif;\r\n" +
                $"  align-items: center;\r\n" +
                $"  background-image: linear-gradient(144deg,#01a362, #00cf7c 50%,#01a362);\r\n" +
                $"  border: 0;\r\n" +
                $"  border-radius: 20px;\r\n" +
                $"  color: #ffffff;\r\n" +
                $"  display: flex;\r\n" +
                $"  font-size: 2.5vw;\r\n" +
                $"  justify-content: center;\r\n" +
                $"  padding: 3px;\r\n" +
                $"  cursor: pointer;\r\n" +
                $"  margin-top: 5vw;\r\n" +
                $"  margin-left: 7.5vw;\r\n" +
                $"}}\r\n" +
                $"\r\n" +
                $"#button span {{\r\n" +
                $"  background-color: #005734;\r\n" +
                $"  padding: 16px 24px;\r\n" +
                $"  border-radius: 20px;\r\n" +
                $"  width: 30vw;\r\n" +
                $"  height: 3vw;\r\n" +
                $"}}\r\n" +
                $"\r\n" +
                $"#button:hover span {{\r\n" +
                $"  background: none;\r\n" +
                $"}}\r\n" +
                $"#body{{\r\n" +
                $"    margin: 5vw;\r\n" +
                $"}}\r\n" +
                $"#hi{{\r\n" +
                $"    font-size: 3.5vw;\r\n" +
                $"    text-align: center;\r\n" +
                $"}}\r\n" +
                $".main-text-size{{\r\n" +
                $"    font-size: 2.5vw;\r\n" +
                $"}}\r\n" +
                $"footer{{\r\n" +
                $"    background-color: #005734;\r\n" +
                $"    height: 15vw;\r\n" +
                $"    margin-left: 20%;\r\n" +
                $"    width: 60%;\r\n" +
                $"}}\r\n" +
                $"</style>\r\n" +
                $"<body>\r\n" +
                $"    <div id=\"main\">\r\n" +
                $"        <img id=\"logo\" src=\"images/logo.png\" alt=\"Logo\">\r\n" +
                $"        <h1 id=\"click\">Click the button, </br>\r\n" +
                $"            to verify your account!</h1>\r\n" +
                $"        <div id=\"body\">\r\n" +
                $"            <h2 id=\"hi\">Hi {auth.username}!</h2>\r\n" +
                $"            <p class=\"main-text-size\">\r\n" +
                $"                Thanks for using Salus - Healthy lifestyle!\r\n" +
                $"                Please click on the button below to confirm your email address.\r\n" +
                $"            </p>\r\n" +
                $"            <strong class=\"main-text-size\">After the verification process is complete:</strong>\r\n" +
                $"            <ul>\r\n" +
                $"                <li class=\"main-text-size\">\r\n" +
                $"                    Please set your personal information! \r\n" +
                $"                </li>\r\n" +
                $"                <li class=\"main-text-size\">\r\n" +
                $"                    Enjoy our app!\r\n" +
                $"                </li>\r\n" +
                $"            </ul>\r\n" +
                $"            <form name=\"myForm\" target=\"_blank\" action='{url}' method=\"post\">\r\n" +
                $"                <button id=\"button\" type=\"submit\" name=\"submit_param\" value=\"submit_value\"><span class=\"text\">VERIFY</span></button>\r\n" +
                $"            </form>\r\n" +
                $"        </div>\r\n" +
                $"    </div>\r\n" +
                $"</body>\r\n" +
                $"</html>";
            //string body =
            //        $"<form name='myForm' target=\"_blank\" action='{url}' method='post'>\r\n" +
            //        $"  <button type=\"submit\" name=\"submit_param\" value=\"submit_value\" class=\"link-button\">\r\n" +
            //        $"    VERIFY\r\n" +
            //        $"  </button>\r\n" +
            //        $"</form>";

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

            client.Send(send_mail);
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
