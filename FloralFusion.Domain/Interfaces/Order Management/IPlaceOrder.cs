using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Order_Management
{
    public interface IPlaceOrder:ICrud<Order,long>
    {
        Task<string> GetOrderStatusAsync(long orderId);

        Task<bool> UpdateOrderStatusAsync(long orderId,string status);

        Task<bool> AddItemToOrder(OrderItem item);

        Task<IEnumerable<OrderStatus>> GetAllOrderStatus();
    }
}
