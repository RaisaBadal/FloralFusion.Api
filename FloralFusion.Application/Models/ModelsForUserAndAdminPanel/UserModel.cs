using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Application.Models.ModelsForUserAndAdminPanel
{
    public class UserModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name should contain only letters and spaces.")]
        [Display(Name = "Name of user")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Surname should contain only letters and spaces.")]
        [Display(Name = "Surname of user")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Display(Name = "Birthday of user")]
        public required DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [StringLength(11, ErrorMessage = "Personal number must be exactly 11 digit", MinimumLength = 11)]
        [Column("Personal_Number")]
        public required string PersonalNumber { get; set; }

        public long? FlowerId { get; set; }

    }
}
