using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("Wishlists")]
    public class Wishlist: AbstractEntity
    { 
        public IEnumerable<WishlistItem> Items { get; set; }

        [ForeignKey(nameof(User))]
        public required string UserId { get; set; }
        public User User { get; set; }
    }
}
