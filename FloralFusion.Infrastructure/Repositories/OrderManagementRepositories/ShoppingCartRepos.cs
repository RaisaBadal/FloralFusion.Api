using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Order_Management;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.OrderManagementRepositories
{
    public class ShoppingCartRepos(FloralFusionDb floralFusionDb) :AbstractRepos<Wishlist>(floralFusionDb),IShoppingCart
    {
        #region GetCartByUserIdAsync
        public async Task<Wishlist> GetCartByUserIdAsync(string userId)
        {
            var res = await dbset.Include(i => i.Items)
                          .Where(i => i.UserId == userId).FirstOrDefaultAsync()
                      ?? throw new ArgumentException($"No wishlist found for user : {userId}");
            return res;

        }

        #endregion

        #region CreateWishlist

        public async Task<long> CreateWishlist(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId,nameof(userId));
            var wishlist = new Wishlist
            {
                UserId = userId
            };
            await dbset.AddAsync(wishlist);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }

        #endregion

        #region AddItemToWishlistItemAsync

        public async Task<WishlistItem> AddItemToWishlistItemAsync(long flowerId, int quantity, string userId)
        {
            if (flowerId < 0 || quantity <= 0) throw new ArgumentException("Invalid flowerId and quantity");
            ArgumentNullException.ThrowIfNull(userId);
            var flower = await floralFusionDb.Flowers.FindAsync(flowerId)
                         ?? throw new ArgumentException($"No flower found for id : {flowerId}");
            var wishlist = await dbset.FirstOrDefaultAsync(i => i.UserId == userId)
                           ?? throw new ArgumentException($"No wishlist found for user {userId}");
            var item = new WishlistItem
            {
                FlowerId = flower.Id,
                Quantity = quantity,
                WishlistId = wishlist.Id
            };
            await floralFusionDb.WishlistItems.AddAsync(item);
            await floralFusionDb.SaveChangesAsync();
            return item;
        }

        #endregion

        #region RemoveItemFromWishlistAsync

        public async Task<bool> RemoveItemFromWishlistAsync(long flowerId, string userId)
        {
            if (flowerId < 0) throw new ArgumentException("Invalid flowerId");
            ArgumentNullException.ThrowIfNull(userId);
            var flower = await floralFusionDb.Flowers.FindAsync(flowerId)
                         ?? throw new ArgumentException($"No flower found for id : {flowerId}");
            var wishlist = await dbset.Include(i=>i.Items).FirstOrDefaultAsync(i => i.UserId == userId)
                           ?? throw new ArgumentException($"No wishlist found for user {userId}");
            var wishlistItem = await floralFusionDb.WishlistItems
                                   .Where(i => i.WishlistId == wishlist.Id && i.FlowerId == flower.Id)
                                   .FirstOrDefaultAsync()
                               ?? throw new ArgumentException("No wishlistItem found");
            floralFusionDb.WishlistItems.Remove(wishlistItem);
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
