
namespace FloralFusion.Application.Models.ModelsForOrders
{
    public class OrderModel
    {

        public required string DeliveryAddress { get; set; }

        public long OrderStatusId { get; set; }

        public long PaymentMethodId { get; set; }

        public long DeliveryOptionId { get; set; }
        

    }
}
