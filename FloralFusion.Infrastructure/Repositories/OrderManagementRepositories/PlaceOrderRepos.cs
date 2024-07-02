using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Order_Management;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.OrderManagementRepositories
{
    public class PlaceOrderRepos(FloralFusionDb floralFusionDb) :AbstractRepos<Order>(floralFusionDb),IPlaceOrder
    {

        #region GetOrderStatusAsync

        public async Task<string> GetOrderStatusAsync(long orderId)
        {
            var res = await dbset.Include(i=>i.OrderStatus).FirstOrDefaultAsync(i => i.Id == orderId)
                      ?? throw new ArgumentException($"No order found by id: {orderId}");
            var orderStatus = res.OrderStatus.StatusName;
            return orderStatus;

        }

        #endregion

        #region UpdateOrderStatusAsync
        public async Task<bool> UpdateOrderStatusAsync(long orderId, string status)
        {
            ArgumentNullException.ThrowIfNull(status);
            if (orderId < 0) throw new ArgumentException("Invalid id");
            var order = await dbset.FindAsync(orderId)
                        ?? throw new ArgumentException($"No order find by id: {orderId}");
            var orderStatus = await floralFusionDb.OrderStatus.FirstOrDefaultAsync(i =>
                i.StatusName.Equals(status, StringComparison.OrdinalIgnoreCase)) 
                              ?? throw new ArgumentException($"No order status find by name: {status}");
            order.OrderStatusId = orderStatus.Id;
            await floralFusionDb.SaveChangesAsync();
            return true;

        }
        #endregion

        #region AddItemToOrder
        public async Task<bool> AddItemToOrder(OrderItem item)
        {
            if (await floralFusionDb.OrderItems.AnyAsync(io => io.FlowerId == item.FlowerId && io.OrderId == item.OrderId))
            {
                var suchFlower=await floralFusionDb.OrderItems.FirstOrDefaultAsync(io=>io.FlowerId==item.FlowerId&&io.OrderId==item.OrderId);
           
                if(suchFlower is not null)
                {
                    suchFlower.Quantity+=item.Quantity;
                }
            }
            else
            {
                await floralFusionDb.OrderItems.AddAsync(item);
            }
            await floralFusionDb.SaveChangesAsync();
            return true;
        }

        #endregion

        #region CRUD
        public async Task<Order> GetByIdAsync(long id)
        {
            var res = await dbset
                .Include(i=>i.OrderStatus)
                .Include(io=>io.orderItems)
                .FirstOrDefaultAsync(i=>i.Id==id);

            if(res is not null)
            {
                return res;
            }
            throw new ArgumentNullException(nameof(id));
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
           return await dbset.ToListAsync();
        }

        public async Task<long> Create(Order entity)
        {
            if (!await floralFusionDb.OrderStatus.AnyAsync(io => io.Id == entity.OrderStatusId))
            {
                throw new ArgumentException("No Such order status exist!");
            }

            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }

        //admin
        public async Task<bool> Update(long id, Order entity)
        {
            var res = await dbset.FindAsync(id);
            if(res is not null)
            {
                res.TrackingNumber = entity.TrackingNumber;
                res.OrderDate=entity.OrderDate;
                res.ShippingDate = entity.ShippingDate;
                res.DeliveryAddress = entity.DeliveryAddress;
               await floralFusionDb.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeleteByIdAsync(long id)
        {
            var res = await dbset.FindAsync(id)
                ?? throw new ArgumentException(nameof(id));
            dbset.Remove(res);
            await floralFusionDb.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            var res = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                ?? throw new ArgumentException(nameof(id));
            var status =await floralFusionDb.OrderStatus.Where(i => i.StatusName == "Deleted").FirstOrDefaultAsync();
            if(status is null) throw new InvalidOperationException("No status found");
            res.OrderStatusId = status.Id;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }

        #endregion

        #region GetAllOrderStatus
        public async Task<IEnumerable<OrderStatus>> GetAllOrderStatus()
        {
            return await floralFusionDb.OrderStatus.ToListAsync();
        }
        #endregion


    }
}
