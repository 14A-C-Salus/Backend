using System.ComponentModel.DataAnnotations;

namespace Authentication.Controllers.Models
{
    public class AuthLoginRequest
    {
        [Required, MinLength(8, ErrorMessage = "Please enter at least 8 characters!"), MaxLength(20, ErrorMessage = "The username may not be longer than 20 characters.")]
        public string username { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage = "Please enter at least 8 characters!"), MaxLength(20, ErrorMessage = "The password may not be longer than 20 characters.")]
        public string password { get; set; } = string.Empty;
    }
}
