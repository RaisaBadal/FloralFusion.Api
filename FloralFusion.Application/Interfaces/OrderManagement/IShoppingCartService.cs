using FloralFusion.Application.Models.ModelsForOrders;

namespace FloralFusion.Application.Interfaces.OrderManagement
{
    public interface IShoppingCartService
    {
        Task<WishlistModel> GetCartByUserIdAsync(string userId);

        Task<long> CreateWishlist(string userId);

        Task<WishlistItemModel> AddItemToWishlistItemAsync(long flowerId, int quantity, string userId);

        Task<bool> RemoveItemFromWishlistAsync(long flowerId, string userId);
    }
}
