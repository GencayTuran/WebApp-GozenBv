using System.ComponentModel.DataAnnotations;

namespace WebApp_GozenBv.ViewModels
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
