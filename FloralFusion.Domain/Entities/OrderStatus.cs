using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("OrdersStatus")]
    public class OrderStatus:AbstractEntity
    {
        public required string StatusName { get; set; }

        public bool IsActive {  get; set; }

        public required string Description {  get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public IEnumerable<Order>Orders { get; set; }
    }
}
