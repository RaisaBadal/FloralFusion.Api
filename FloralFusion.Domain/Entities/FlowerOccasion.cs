using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("FlowerOccasions")]
    [Index(nameof(OccasionType),IsUnique = true)]
    public class FlowerOccasion: AbstractEntity
    {

        [Required]
        [StringLength(50, ErrorMessage = "Such flower OccasionType is not valid", MinimumLength = 2)]
        public required string OccasionType { get; set; }

        public IEnumerable<Flower> Flower { get; set; }
    }
}
