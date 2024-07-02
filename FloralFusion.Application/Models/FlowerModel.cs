using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models
{
    public class FlowerModel
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
        public long OccasionId { get; set; }


        [Required]
        [Column("FlowerCategoryId")]
        public long CategoryId { get; set; }

        public bool Availability { get; set; }

        public bool Featured { get; set; }

        public int Quantity { get; set; }

    }
}
