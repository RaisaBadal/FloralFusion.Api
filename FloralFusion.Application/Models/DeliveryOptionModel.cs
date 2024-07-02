using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models
{
    public class DeliveryOptionModel
    {
        [Column("DeliveryOptionName")]
        [StringLength(50, ErrorMessage = "Such delivery option name is not valid", MinimumLength = 4)]
        public required string OptionName { get; set; }

        [Column("DeliveryOptionDescription")]
        [StringLength(500, ErrorMessage = "Such delivery option description is not valid", MinimumLength = 4)]
        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? MinDeliveryTime { get; set; }

        public int? MaxDeliveryTime { get; set; }

        [StringLength(500, ErrorMessage = "Such delivery Additional information is not valid", MinimumLength = 4)]
        public string? AdditionalInformation { get; set; }
    }
}
