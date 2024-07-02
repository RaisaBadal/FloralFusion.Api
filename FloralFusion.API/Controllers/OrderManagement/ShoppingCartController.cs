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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMemoryCache _memoryCache;

        public ShoppingCartController(IShoppingCartService _shoppingCartService, IMemoryCache _memoryCache)
        {
            this._shoppingCartService = _shoppingCartService;
            this._memoryCache = _memoryCache;
        }

        //shesuli useris wishlists daabrunebs
        [HttpGet]
        [Route("[action]")]
        public async Task<Response<WishlistModel>> GetCartByUserId()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var cacheKey = $"users: {userId} wishlist";
            if(_memoryCache.TryGetValue(cacheKey, out WishlistModel? cache)&&cache is not null) 
                return Response<WishlistModel>.Ok(cache);

            var res = await _shoppingCartService.GetCartByUserIdAsync(userId);
            if (res is null) return Response<WishlistModel>.Error(ErrorKeys.NotFound);

            _memoryCache.Set(cacheKey, res,TimeSpan.FromMinutes(10));
            return Response<WishlistModel>.Ok(res);
        }

        [HttpGet]
        [Route(nameof(CreateWishlist))]
        public async Task<Response<long>> CreateWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _shoppingCartService.CreateWishlist(userId);
            return res<0 ? Response<long>.Error(ErrorKeys.UnSuccessFullInsert) :
                Response<long>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/flowerId/{flowerId}")]
        public async Task<Response<WishlistItemModel>> AddItemToWishlistItem([FromRoute]long flowerId, [FromBody]int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _shoppingCartService.AddItemToWishlistItemAsync(flowerId, quantity,userId);

            return res is not null ? Response< WishlistItemModel>.Ok(res) :
                Response<WishlistItemModel>.Error(ErrorKeys.UnSuccessFullInsert);
        }

        [HttpDelete]
        [Route("[action]/flowerId/{flowerId}")]
        public async Task<Response<bool>> RemoveItemFromWishlist(long flowerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException(ErrorKeys.BadRequest);

            var res=await _shoppingCartService.RemoveItemFromWishlistAsync(flowerId,userId);
            return res ? Response<bool>.Ok(res) :
                Response<bool>.Error(ErrorKeys.BadRequest);
        }
    }
}
