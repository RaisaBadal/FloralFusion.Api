using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models.ModelsForUserAndAdminPanel
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Old Password is required")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string NewPassword { get; set; }
    }
}
