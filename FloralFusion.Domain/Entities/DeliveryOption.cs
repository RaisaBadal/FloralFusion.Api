using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("DeliveryOptions")]
    [Index(nameof(Price))]
    [Index(nameof(MinDeliveryTime))]
    [Index(nameof(MaxDeliveryTime))]
    public class DeliveryOption:AbstractEntity
    {
        [Column("DeliveryOptionName")]
        [StringLength(50, ErrorMessage = "Such delivery option name is not valid", MinimumLength = 4)]
        public required string OptionName { get; set; }

        public string? Description {  get; set; }

        public bool IsActive { get; set; } = true;

        public decimal? Price { get; set; }

        public int? MinDeliveryTime {  get; set; }

        public int? MaxDeliveryTime { get; set;}

        public string? AdditionalInformation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get;set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
