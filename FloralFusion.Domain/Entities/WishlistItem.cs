using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("WishlistItems")]
    public class WishlistItem:AbstractEntity
    { 

        [ForeignKey(nameof(Flower))]
        public long FlowerId { get; set; }
        public Flower Flower { get; set; }


        [Column("Flower_Quantity")]
        public int Quantity { get; set; }


        [ForeignKey(nameof(Wishlist))]
        public long WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
