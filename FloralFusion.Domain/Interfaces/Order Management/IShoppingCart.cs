using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Order_Management
{
    public interface IShoppingCart
    {
        Task<Wishlist> GetCartByUserIdAsync(string userId);

        Task<long> CreateWishlist(string userId);

        Task<WishlistItem> AddItemToWishlistItemAsync(long flowerId,int quantity, string userId);

        Task<bool> RemoveItemFromWishlistAsync(long flowerId, string userId);
    }
}
