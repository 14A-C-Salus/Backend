namespace Salus.Models.Requests
{
    public class AuthResetPasswordRequest
    {
        [Required, MinLength(8, ErrorMessage = "Please enter at least 8 characters!"), MaxLength(20, ErrorMessage = "The password may not be longer than 20 characters.")]
        public string password { get; set; } = string.Empty;
        [Required, Compare("password")]
        public string confirmPassword { get; set; } = string.Empty;

    }
}
