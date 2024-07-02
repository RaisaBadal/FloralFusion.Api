using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Notifications;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.NotificationRepositories
{
    public class NotificationsRepos(FloralFusionDb floralFusionDb)
        : AbstractRepos<Notification>(floralFusionDb), INotifications
    {
        #region GetByIdAsync

        public async Task<Notification> GetByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No notification found by id: {id}");
            return res;
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }
        #endregion

        #region Create

        public async Task<long> Create(Notification entity)
        {
            ArgumentNullException.ThrowIfNull(entity,nameof(entity));
            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }
        #endregion

        #region Update
        public async Task<bool> Update(long id, Notification entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No notification found by id: {id}");
            res.Message=entity.Message;
            res.NotificationTypeId=entity.NotificationTypeId;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteByIdAsync

        public async Task DeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No notification found by id: {id}");
            dbset.Remove(res);
            await floralFusionDb.SaveChangesAsync();
        }

        #endregion

        #region SoftDeleteByIdAsync

        public async Task<bool> SoftDeleteByIdAsync(long id)
        {

            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No notification found by id: {id}");
            res.IsActive = false;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion

        #region AttachNotificationToUserAsync
        public async Task<bool> AttachNotificationToUserAsync(UserNotification userNotification)
        {
            var context = await floralFusionDb.UserNotifications
                .AnyAsync(i => i.UserId == userNotification.UserId && i.NotificationId == userNotification.NotificationId);
            if (context) throw new ArgumentException("Such record already exist in BookBridgeDB");
            await floralFusionDb.UserNotifications.AddAsync(userNotification);
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion

        #region MyRegion
        public async Task<IEnumerable<UserNotification>> GetAllNotificationAsync()
        {
            return await floralFusionDb.UserNotifications.AsNoTracking().ToListAsync();
        }
        #endregion
    }
}
