using FloralFusion.Domain.Data;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Domain.Interfaces.Admin;
using FloralFusion.Domain.Interfaces.DeliveryManagement;
using FloralFusion.Domain.Interfaces.Flowers_Management;
using FloralFusion.Domain.Interfaces.Notifications;
using FloralFusion.Domain.Interfaces.Order_Management;
using FloralFusion.Domain.Interfaces.Payment_Processing;
using FloralFusion.Domain.Interfaces.Reports_and_Analytics;
using FloralFusion.Domain.Interfaces.Review_and_Rating_System;
using FloralFusion.Infrastructure.Repositories.AdminRepositories;
using FloralFusion.Infrastructure.Repositories.DeliveryManagementRepositories;
using FloralFusion.Infrastructure.Repositories.FlowersManagementRepositories;
using FloralFusion.Infrastructure.Repositories.NotificationRepositories;
using FloralFusion.Infrastructure.Repositories.OrderManagementRepositories;
using FloralFusion.Infrastructure.Repositories.PaymentMethodRepositories;
using FloralFusion.Infrastructure.Repositories.ReportsAndAnalyticsRepositories;
using FloralFusion.Infrastructure.Repositories.ReviewAndRatingRepositories;

namespace FloralFusion.Infrastructure.Repositories
{
    public class UniteOfWork(FloralFusionDb floralFusionDb) : IUniteOfWork
    {
        public IFlower FlowerRepository => new FlowerRepos(floralFusionDb);
        public IManageFlowers ManageFlowers => new ManageFlowerRepos(floralFusionDb);
        public IManageOrders ManageOrders => new ManageOrdersRepos(floralFusionDb);
        public IDeliveryOptions DeliveryOptions => new DeliveryOptionsRepositories(floralFusionDb);
        public IFlowerCatalogManagement FlowerCatalogManagement => new FlowerCatalogManagementRepos(floralFusionDb);
        public INotifications Notifications => new NotificationsRepos(floralFusionDb);
        public IPlaceOrder PlaceOrder => new PlaceOrderRepos(floralFusionDb);
        public IShoppingCart ShoppingCart => new ShoppingCartRepos(floralFusionDb);
        public IPaymentMethods PaymentMethods => new PaymentRepos(floralFusionDb);
        public ISalesReports SalesReports => new SalesReportsRepos(floralFusionDb);
        public IUserReviewsAndRatings UserReviewsAndRatings => new UserReviewsAndRatingsRepos(floralFusionDb);

        #region CheckAndCommit
        public async Task CheckAndCommit()
        {
            try
            {
                await floralFusionDb.SaveChangesAsync();
                await floralFusionDb.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await floralFusionDb.Database.RollbackTransactionAsync();
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            floralFusionDb.Dispose(); //wyvets bazastan kavshirs
        }
        #endregion

        #region SaveChanges
        public async Task SaveChanges()
        {
           await floralFusionDb.SaveChangesAsync();
        }
        #endregion
    }
}
