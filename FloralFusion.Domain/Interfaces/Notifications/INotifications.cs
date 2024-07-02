using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Notifications
{
    public interface INotifications:ICrud<Notification,long>
    {
        Task<bool> AttachNotificationToUserAsync(UserNotification userNotification);
        Task<IEnumerable<UserNotification>> GetAllNotificationAsync();
    }
}
