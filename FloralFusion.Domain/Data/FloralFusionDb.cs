using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Entities.LogEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Data
{
    public class FloralFusionDb(DbContextOptions<FloralFusionDb> db) : IdentityDbContext<User>(db)
    {
       
        public virtual DbSet<Flower>Flowers { get; set; }

        public virtual DbSet<FlowerOccasion> FlowerOccasion {  get; set; }

        public virtual DbSet<FlowerCategory> FlowerCategory { get; set; }

        public virtual DbSet<Order>Orders { get; set; }

        public virtual DbSet<DeliveryOption>DeliveryOptions { get; set; }

        public virtual DbSet<OrderStatus> OrderStatus { get; set; }

        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

        public virtual DbSet<WishlistItem> WishlistItems { get; set; }

        public virtual DbSet<Wishlist> Wishlists { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<NotificationType> NotificationTypes { get; set; }

        public virtual DbSet<OrderItem>OrderItems { get; set; }

        public virtual DbSet<Reviews> Reviews { get; set; }

        public virtual DbSet<SalesReport> SalesReports { get; set; }

        public virtual DbSet<SearchHistory> SearchHistory { get; set; }

        public virtual DbSet<UserNotification>UserNotifications { get; set; }

        public virtual DbSet<Logs> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Flower>()
                .HasOne(i => i.FlowerOccasion)
                .WithMany(i => i.Flower);

            builder.Entity<Flower>()
                .HasOne(i=>i.FlowerCategory)
                .WithMany(i=>i.Flower);

            builder.Entity<Flower>()
                .HasMany(i => i.User)
                .WithMany(i => i.Flower);

            builder.Entity<User>()
                .HasMany(i => i.CartModels)
                .WithOne(i => i.User);

            builder.Entity<Notification>()
                .HasOne(i => i.NotificationType)
                .WithMany(i => i.Notifications);

            builder.Entity<User>()
                .HasMany(i => i.Reviews)
                .WithOne(i => i.User);

            builder.Entity<Reviews>()
                .HasOne(i => i.Flower)
                .WithMany(i => i.Reviews);

            builder.Entity<SearchHistory>()
                .HasOne(i => i.User)
                .WithMany(i => i.SearchHistories);

        }
    }
}
