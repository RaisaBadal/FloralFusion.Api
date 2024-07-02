using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.OrderManagement;
using FloralFusion.Application.Models.ModelsForOrders;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FloralFusion.API.Controllers.OrderManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaceOrderController : ControllerBase
    {
        private readonly IPlaceOrderService _placeOrderService;
        private readonly IMemoryCache _memoryCache;

        public PlaceOrderController(IPlaceOrderService _placeOrderService, IMemoryCache _memoryCache)
        {
            this._placeOrderService = _placeOrderService;
            this._memoryCache = _memoryCache;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<long>> PlaceOrder(OrderModel model)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);
            var res = await _placeOrderService.PlaceOrderAsync(model, userId);
            return res < 0 ? Response<long>.Error(ErrorKeys.UnSuccessFullInsert)
                : Response<long>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/orderId/{orderId}")]
        public async Task<Response<string>> GetOrderStatus([FromRoute] long orderId)
        {
            var res = await _placeOrderService.GetOrderStatusAsync(orderId);
            if (string.IsNullOrEmpty(res)) return Response<string>.Error(ErrorKeys.NotFound);
            return Response<string>.Ok(res);
        }

        //admins da gadamzidi kompaniis tanamshromels sheedzleba update 
        [HttpPatch]
        [Route("[action]/orderId/{orderId}/statusId/{statusId}")]

        public async Task<Response<bool>> UpdateOrderStatus([FromRoute] long orderId, [FromRoute] long statusId)
        {
            var res = await _placeOrderService.UpdateOrderStatusAsync(orderId, statusId);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.UnSuccessFullUpdate);
        }

        [HttpPatch]
        [Route("[action]/orderId/{orderId}")]
        public async Task<Response<bool>> MarkAsDelivery([FromRoute] long orderId)
        {
            var res = await _placeOrderService.MarkAsDelivery(orderId);
            return res ? Response<bool>.Ok(res)
             : Response<bool>.Error(ErrorKeys.UnSuccessFullUpdate);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<bool>> AddItemToOrder([FromBody]OrderItemModel entity)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _placeOrderService.AddItemToOrder(entity);
            return res ? Response<bool>.Ok(res)
           : Response<bool>.Error(ErrorKeys.UnSuccessFullInsert);
        }

        [HttpDelete]
        [Route("[action]/oderId/{orderId}/orderItemId/{orderItemId}")]
        public async Task<Response<bool>> RemoveItemFromOrder(long orderId, long orderItemId)
        {
            var res = await _placeOrderService.RemoveItemFromOrder(orderId, orderItemId);
            return res ? Response<bool>.Ok(res)
             : Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}