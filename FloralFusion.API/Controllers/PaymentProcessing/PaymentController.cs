using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.PaymentMethods;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FloralFusion.API.Controllers.PaymentProcessing
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ADMIN")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMemoryCache _memoryCache;

        public PaymentController(IPaymentService _paymentService, IMemoryCache _memoryCache)
        {
            this._paymentService = _paymentService;
            this._memoryCache = _memoryCache;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<long>> Create([FromBody] PaymentModel entity)
        {
            if (!ModelState.IsValid || entity is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await _paymentService.CreateAsync(entity);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task DeleteById([FromRoute] long id)
        {
            await _paymentService.DeleteByIdAsync(id);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<PaymentModel>>> AllPaymentMethod()
        {
            const string cacheKey = "AllPaymentMethod";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<PaymentModel>? result) && result is not null)
                return Response<IEnumerable<PaymentModel>>.Ok(result);

            var res = await _paymentService.GetAllAsync();
            if (!res.Any()) return Response<IEnumerable<PaymentModel>>.Error(ErrorKeys.BadRequest);

            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<PaymentModel>>.Ok(res);

        }


        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<PaymentModel>> GetPaymentById([FromRoute] long id)
        {
            var cacheKey = $"payment method: {id}";
            if (_memoryCache.TryGetValue(cacheKey, out PaymentModel? model) && model is not null)
                return Response<PaymentModel>.Ok(model);

            var res = await _paymentService.GetByIdAsync(id);
            if (res is null) return Response<PaymentModel>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<PaymentModel>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteById([FromRoute] long id)
        {
            var res = await _paymentService.SoftDeleteByIdAsync(id);
            return res ? Response<bool>.Ok(res) :
                Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> Update([FromRoute] long id, PaymentModel entity)
        {
            if (!ModelState.IsValid || entity is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await _paymentService.UpdateAsync(id, entity);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}
