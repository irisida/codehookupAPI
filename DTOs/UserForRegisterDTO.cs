using System.ComponentModel.DataAnnotations;

namespace matcher.API.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 5, ErrorMessage = "You must specify a password of 6-64 characters")]
        public string Password { get; set; }
    }
}