using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models.ModelsForUserAndAdminPanel
{
    public class RoleModel
    {
        [Required(ErrorMessage = "the field is required")]
        public required string Name { get; set; }
    }
}
