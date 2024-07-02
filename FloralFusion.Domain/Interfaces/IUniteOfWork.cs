using FloralFusion.Domain.Interfaces.Admin;
using FloralFusion.Domain.Interfaces.DeliveryManagement;
using FloralFusion.Domain.Interfaces.Flowers_Management;
using FloralFusion.Domain.Interfaces.Notifications;
using FloralFusion.Domain.Interfaces.Order_Management;
using FloralFusion.Domain.Interfaces.Payment_Processing;
using FloralFusion.Domain.Interfaces.Reports_and_Analytics;
using FloralFusion.Domain.Interfaces.Review_and_Rating_System;

namespace FloralFusion.Domain.Interfaces
{
    public interface IUniteOfWork:IDisposable
    {
        public IFlower FlowerRepository { get; }

        public IManageFlowers ManageFlowers { get; }

        public IManageOrders ManageOrders { get; }  

        public IDeliveryOptions DeliveryOptions { get; }

        public IFlowerCatalogManagement FlowerCatalogManagement { get; }

        public INotifications Notifications { get; }

        public IPlaceOrder PlaceOrder { get; }

        public IShoppingCart ShoppingCart { get; }

        public IPaymentMethods PaymentMethods { get; }

        public ISalesReports SalesReports { get; }

        public IUserReviewsAndRatings UserReviewsAndRatings { get; }

        Task SaveChanges();

        Task CheckAndCommit();
    }
}
