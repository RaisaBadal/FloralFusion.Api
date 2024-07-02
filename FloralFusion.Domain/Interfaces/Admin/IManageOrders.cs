using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Admin
{
    public interface IManageOrders
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(long orderId);
        Task<bool> UpdateOrderStatusAsync(long orderId, long orderStatusId);
    }
}
