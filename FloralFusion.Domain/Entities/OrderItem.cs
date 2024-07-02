using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("OrderItems")]
    public class OrderItem: AbstractEntity
    {
        [ForeignKey(nameof(Flower))]
        public long FlowerId {  get; set; }

        public Flower Flower { get; set; }

        [Column("QuantityOfItem")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }
        public Order Order { get; set; }

    }
}
