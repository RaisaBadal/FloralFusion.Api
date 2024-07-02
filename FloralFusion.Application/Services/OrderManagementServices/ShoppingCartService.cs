using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.OrderManagement;
using FloralFusion.Application.Models.ModelsForOrders;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.OrderManagementServices
{
    public class ShoppingCartService : AbstractClass, IShoppingCartService
    {
        public ShoppingCartService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region AddItemToWishlistItemAsync
        public async Task<WishlistItemModel> AddItemToWishlistItemAsync(long flowerId, int quantity, string userId)
        {
            if (flowerId < 0 || quantity < 0 || string.IsNullOrEmpty(userId))
                throw new InvalidOperationException(ErrorKeys.ArgumentNull);
            var res=await uniteOfWork.ShoppingCart.AddItemToWishlistItemAsync(flowerId, quantity, userId);
            var mapped = mapper.Map<WishlistItemModel>(res)
                ?? throw new GeneralException(ErrorKeys.Mapped);
            return mapped;

        }
        #endregion

        #region CreateWishlist

        public async Task<long> CreateWishlist(string userId)
        {
            var res = await uniteOfWork.ShoppingCart.CreateWishlist(userId);
            return res;
        }
        #endregion

        #region GetCartByUserIdAsync
        public async Task<WishlistModel> GetCartByUserIdAsync(string userId)
        {
            var res=await uniteOfWork.ShoppingCart.GetCartByUserIdAsync(userId);
            var mapped = mapper.Map<WishlistModel>(res)
                ?? throw new GeneralException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region RemoveItemFromWishlistAsync

        public async Task<bool> RemoveItemFromWishlistAsync(long flowerId, string userId)
        {
            if(flowerId<0||string.IsNullOrEmpty(userId)) throw new GeneralException(ErrorKeys.ArgumentNull);
            var res = await uniteOfWork.ShoppingCart.RemoveItemFromWishlistAsync(flowerId, userId);
            return res;
        }
        #endregion
    }
}
