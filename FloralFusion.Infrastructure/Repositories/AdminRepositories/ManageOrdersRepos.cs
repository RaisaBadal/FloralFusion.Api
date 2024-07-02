using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.AdminRepositories
{
    public class ManageOrdersRepos(FloralFusionDb floralFusionDb) : AbstractRepos<Order>(floralFusionDb), IManageOrders
    {
        #region GetAllOrdersAsync
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dbset.ToListAsync();
        }
        #endregion

        #region GetOrderByIdAsync

        public async Task<Order> GetOrderByIdAsync(long orderId)
        {
            var order = await dbset.FirstOrDefaultAsync(i => i.Id == orderId);
            return order ??
                throw new ArgumentException($"No order find by id: {orderId}");

        }

        #endregion

        #region UpdateOrderStatusAsync
        public async Task<bool> UpdateOrderStatusAsync(long orderId, long orderStatusId)
        {
            var order = await dbset.FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null) throw new ArgumentException($"No order find by id: {orderId}");
            var orderStatus = await floralFusionDb.OrderStatus.FirstOrDefaultAsync(i => i.Id == orderStatusId);
            if(orderStatus==null) throw new ArgumentException($"No order status find by id: {orderStatusId}");
            order.OrderStatus = orderStatus;
            await floralFusionDb.SaveChangesAsync();
            return true;    
        }
        #endregion
    }
}
