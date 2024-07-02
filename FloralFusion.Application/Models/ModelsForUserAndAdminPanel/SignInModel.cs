using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models.ModelsForUserAndAdminPanel
{
    public class SignInModel
    {
        [Required(ErrorMessage = "the field is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "the field is required")]
        public required string Password { get; set; }
    }
}
