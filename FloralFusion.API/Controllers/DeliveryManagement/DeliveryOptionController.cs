using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.DeliveryOptions;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FloralFusion.API.Controllers.DeliveryManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ADMIN")]
    public class DeliveryOptionController : ControllerBase
    {
        private readonly IDeliveryOptionService deliveryOptions;
        private readonly IMemoryCache memoryCache;

        public DeliveryOptionController(IDeliveryOptionService deliveryOptions, IMemoryCache memoryCache)
        {
            this.deliveryOptions = deliveryOptions;
            this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<long>> Create([FromBody]DeliveryOptionModel entity)
        {
            if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await deliveryOptions.CreateAsync(entity);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task DeleteById([FromRoute]long id)
        {
            await deliveryOptions.DeleteByIdAsync(id);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<DeliveryOptionModel>>> AllDeliveryOption()
        {
            const string cacheKey = "AllDeliveryOption";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<DeliveryOptionModel>? result) && result is not null)
                return Response<IEnumerable<DeliveryOptionModel>>.Ok(result);

            var res = await deliveryOptions.GetAllAsync();
            if (!res.Any()) return Response<IEnumerable<DeliveryOptionModel>>.Error(ErrorKeys.BadRequest);

            memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<DeliveryOptionModel>>.Ok(res);

        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<DeliveryOptionModel>> GetDeliveryById([FromRoute] long id)
        {
            var cacheKey = $"DeliveryOption: {id}";
            if(memoryCache.TryGetValue(cacheKey,out DeliveryOptionModel? model) &&model is not null)
                return Response<DeliveryOptionModel>.Ok(model);

            var res=await deliveryOptions.GetByIdAsync(id);
            if(res is null) return Response<DeliveryOptionModel>.Error(ErrorKeys.NotFound);
            memoryCache.Set(cacheKey,res, TimeSpan.FromMinutes(15));
            return Response<DeliveryOptionModel>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteById([FromRoute]long id)
        {
            var res=await deliveryOptions.SoftDeleteByIdAsync(id);
            return res ? Response<bool>.Ok(res) :
                Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> Update([FromRoute]long id, DeliveryOptionModel entity)
        {

            if (!ModelState.IsValid || entity is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await deliveryOptions.UpdateAsync(id, entity);
            return res ? Response<bool>.Ok(res) 
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}
