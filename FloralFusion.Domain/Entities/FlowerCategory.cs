using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("FlowerCategories")]
    [Index(nameof(CategoryName),IsUnique = true)]
    public class FlowerCategory: AbstractEntity
    {

        [Required]
        [StringLength(50, ErrorMessage = "Such flower CategoryName is not valid", MinimumLength = 2)]
        public required string CategoryName { get; set; }

        public IEnumerable<Flower> Flower { get; set; }
    }
}
