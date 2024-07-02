using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Domain.Entities
{
    [Table("Users")]
    public class User:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(11, ErrorMessage = "Personal number must be exactly 11 digit", MinimumLength = 11)]
        [Column("Personal_Number")]
        public required string PersonalNumber { get; set; }

        public long? FlowerId { get; set; }


        public IEnumerable<Flower> Flower { get; set; } 

        public IEnumerable<Order>Orders { get; set; } 

        public IEnumerable<Wishlist>CartModels { get; set; }

        public IEnumerable<Reviews> Reviews { get; set; }   

        public IEnumerable<SearchHistory>SearchHistories { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
