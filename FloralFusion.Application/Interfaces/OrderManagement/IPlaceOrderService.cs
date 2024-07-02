using FloralFusion.Application.Models.ModelsForOrders;

namespace FloralFusion.Application.Interfaces.OrderManagement
{
    public interface IPlaceOrderService
    {
        Task<long> PlaceOrderAsync(OrderModel model, string userId);

        Task<string> GetOrderStatusAsync(long orderId);

        Task<bool> UpdateOrderStatusAsync(long orderId, long statusId);

        Task<bool> MarkAsDelivery(long orderId);

        Task<bool> AddItemToOrder(OrderItemModel entity);

        Task<bool> RemoveItemFromOrder(long orderId,long orderItemId);
    }
}
