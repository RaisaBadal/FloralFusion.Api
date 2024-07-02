using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("PaymentMethods")]
    public class PaymentMethod:AbstractEntity
    {
        public required string MethodName { get; set; }

        public required string Description {  get; set; }

        public bool IsActive { get; set; } = true;

        public decimal? FeePercentage {  get; set; }

        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public DateTime? UpdatedAt { get; set;}

        public IEnumerable<Order> Orders { get; set; }
    }
}
