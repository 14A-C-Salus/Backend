namespace Authentication.Controllers.Models.AuthModels
{
    public class Auth
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; } = new byte[32];
        public byte[] passwordSalt { get; set; } = new byte[32];
        public string? verificationToken { get; set; }
        public DateTime? date { get; set; }
        public string? passwordResetToken { get; set; }
        public DateTime? resetTokenExpires { get; set; }

        //Connection with auth
        public virtual UserProfile userProfile { get; set; }
    }
}
