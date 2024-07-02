using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("Orders")]
    [Index(nameof(TrackingNumber))]
    public class Order:AbstractEntity
    {
        public DateTime OrderDate { get; set; }

        public required string DeliveryAddress {  get; set; }

        public decimal TotalPrice {  get; set; }

        [StringLength(16)]
        public required string TrackingNumber {  get; set; }

        public DateTime? ShippingDate { get; set; }


        [ForeignKey(nameof(PaymentMethod))]
        public long PaymentMethodId {  get; set; }

        public PaymentMethod PaymentMethod { get; set; }


        [ForeignKey(nameof(DeliveryOption))]
        public long DeliveryOptionId { get; set; }

        public DeliveryOption DeliveryOption { get; set; }


        [ForeignKey(nameof(OrderStatus))]
        public long OrderStatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId {  get; set; }

        public User User { get; set; }

        public List<OrderItem> orderItems { get; set; }
    }
}
