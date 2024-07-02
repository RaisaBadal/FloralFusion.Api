using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("Flowers")]
    [Index(nameof(Name),IsUnique = true)]
    [Index(nameof(Price),IsUnique = true)]
    public class Flower: AbstractEntity
    {
   
        [Required]
        [StringLength(50, ErrorMessage = "Such flower Name is not valid", MinimumLength = 2)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Such flower color is not valid", MinimumLength = 3)]
        public required string Color { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(50, ErrorMessage = "Such flower description is not valid", MinimumLength = 5)]
        public string? Description { get; set; }

        [Required]
        [Column("FlowerOccasionId")]
        [ForeignKey("FlowerOccasion")]
        public long OccasionId {  get; set; }  

        public required FlowerOccasion FlowerOccasion { get; set; }

        [Required]
        [Column("FlowerCategoryId")]
        [ForeignKey("FlowerCategory")]
        public long CategoryId {  get; set; }

        public required FlowerCategory FlowerCategory { get; set; }

        public bool Availability {  get; set; }

        public bool Featured {  get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; } = true;


        public List<User> User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public IEnumerable<Reviews> Reviews { get; set; }
    }
}
