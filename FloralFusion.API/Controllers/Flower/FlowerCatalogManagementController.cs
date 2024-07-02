using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.Flowers;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using FloralFusion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FloralFusion.API.Controllers.Flower
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="SELLER")]
    public class FlowerCatalogManagementController : ControllerBase
    {

        private readonly IFlowerCatalogManagementService _flowerService;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<User> _userManager;

        public FlowerCatalogManagementController(IFlowerCatalogManagementService _flowerService, IMemoryCache _memoryCache, UserManager<User> _userManager)
        {
            this._flowerService = _flowerService;
            this._memoryCache = _memoryCache;
            this._userManager = _userManager;
        }

        //funqciebi ekutvis gamyidvels, gamyidvelis rolit shemosuli shedzlebs am funqciebis gamoyenebas

        [HttpPost]
        [Route(nameof(AddFlower))]
        public async Task<Response<long>> AddFlower([FromBody]FlowerModel entity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            if (string.IsNullOrEmpty(userId))
            {
                return Response<long>.Error(ErrorKeys.Unauthorized);
            }
            
            var res=await _flowerService.Create(entity,userId);
            return res<0 ? Response<long>.Error(ErrorKeys.UnSuccessFullInsert)
                : Response<long>.Ok(res);

        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task DeleteById([FromRoute]long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            await _flowerService.DeleteByIdAsync(id,userId);
        }

        //return authorized users(sellers) flowers

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var cacheKey = $"All flowers with seller: {userId}";

            if(_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? result)&& result.Any())
                return Response<IEnumerable<FlowerModel>>.Ok(result);

            var res = await _flowerService.GetAllAsync(userId);
            if(!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);

            _memoryCache.Set(cacheKey, res,TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);

        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<FlowerModel>> GetById([FromRoute]long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var cacheKey = $"user: {userId} flower : {id}";

            if (_memoryCache.TryGetValue(cacheKey, out FlowerModel? result) && result is not null)
                return Response<FlowerModel>.Ok(result);

            var res = await _flowerService.GetByIdAsync(id,userId);
            if (res is null) return Response<FlowerModel>.Error(ErrorKeys.NotFound);

            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<FlowerModel>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> SoftDeleteByIdAsync([FromRoute]long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _flowerService.SoftDeleteByIdAsync(id, userId);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<Response<bool>> Update([FromRoute]long id, [FromBody]FlowerModel entity)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.ArgumentNull);

            if(!ModelState.IsValid) return Response<bool>.Error(ErrorKeys.BadRequest);

            var res=await _flowerService.Update(id, entity,userId);
            return res ? Response<bool>.Ok(res)
              : Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}
