using System.ComponentModel.DataAnnotations;

namespace RapidPayAPI.Services.Users.Models
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
