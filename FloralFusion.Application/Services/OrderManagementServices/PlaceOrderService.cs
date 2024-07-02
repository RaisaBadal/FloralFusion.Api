using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.OrderManagement;
using FloralFusion.Application.Models.ModelsForOrders;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Identity;

namespace FloralFusion.Application.Services.OrderManagementServices
{
    public class PlaceOrderService : AbstractClass, IPlaceOrderService
    {
        private readonly UserManager<User> _userManager;    
        public PlaceOrderService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService, UserManager<User> _userManager) : base(uniteOfWork, mapper, smtpService)
        {
            this._userManager = _userManager;
        }

        #region AddItemToOrder
        public async Task<bool> AddItemToOrder(OrderItemModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                var order = await uniteOfWork.PlaceOrder.GetByIdAsync(entity.OrderId);
               
                var flower = await uniteOfWork.FlowerCatalogManagement.GetByIdAsync(entity.FlowerId);
                if(order is null ||flower is null)
                {
                    throw new ArgumentException(ErrorKeys.NotFound);
                }

                var mapped = mapper.Map<OrderItem>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                await uniteOfWork.PlaceOrder.AddItemToOrder(mapped);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetOrderStatusAsync
        public async Task<string> GetOrderStatusAsync(long orderId)
        {
            try
            {
                if(orderId<0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var order = await uniteOfWork.PlaceOrder.GetByIdAsync(orderId)
                    ?? throw new GeneralException(ErrorKeys.NotFound);
                return order.OrderStatus.StatusName;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region MarkAsDelivery
        public async Task<bool> MarkAsDelivery(long orderId)
        {
            try
            {
                if (orderId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var order = await uniteOfWork.PlaceOrder.GetByIdAsync(orderId)
                    ?? throw new GeneralException(ErrorKeys.NotFound);
                var status=await uniteOfWork.PlaceOrder.GetAllOrderStatus();
                var shippingStatus = status.FirstOrDefault(i => i.StatusName.Equals("Delivered", StringComparison.OrdinalIgnoreCase));
                if (shippingStatus is null) throw new GeneralException(ErrorKeys.NotFound);

                order.ShippingDate = DateTime.Now;
                order.OrderStatusId = shippingStatus.Id;

                await uniteOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion

        #region PlaceOrderAsync
        public async Task<long> PlaceOrderAsync(OrderModel model,string userId)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if(await uniteOfWork.DeliveryOptions.GetByIdAsync(model.DeliveryOptionId) is null)
                 throw new GeneralException(ErrorKeys.NotFound);
            if(await uniteOfWork.PaymentMethods.GetByIdAsync(model.PaymentMethodId) is null)
                   throw new GeneralException(ErrorKeys.NotFound);

            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new ArgumentException(ErrorKeys.NotFound);

            var mapped=mapper.Map<Order>(model) ??
                throw new GeneralException(ErrorKeys.Mapped);
            mapped.UserId= user.Id;
            mapped.OrderDate=DateTime.Now;
            var res = await uniteOfWork.PlaceOrder.Create(mapped);
            return res;
        }
        #endregion

        #region RemoveItemFromOrder
        public async Task<bool> RemoveItemFromOrder(long orderId, long orderItemId)
        {
            var order = await uniteOfWork.PlaceOrder.GetByIdAsync(orderId)
               ?? throw new InvalidOperationException("Order does not exist.");

            var orderItem = order.orderItems.Where(io => io.Id == orderId).FirstOrDefault()
             ?? throw new InvalidOperationException("Order item does not exist.");

            order.orderItems.Remove(orderItem);
            await uniteOfWork.SaveChanges();
            return true;
        }
        #endregion

        #region UpdateOrderStatusAsync
        public async Task<bool> UpdateOrderStatusAsync(long orderId, long statusId)
        {
            var statusObj = await uniteOfWork.PlaceOrder.GetAllOrderStatus()
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var status = statusObj.FirstOrDefault(i => i.Id == statusId)
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var order=await uniteOfWork.PlaceOrder.GetByIdAsync(orderId)
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            order.OrderStatusId = status.Id;
            await uniteOfWork.SaveChanges();
            return true;
        }
        #endregion
    }
}
