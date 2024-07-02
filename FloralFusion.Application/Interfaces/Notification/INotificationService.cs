using FloralFusion.Application.Models;

namespace FloralFusion.Application.Interfaces.Notification
{
    public interface INotificationService
    {
        Task<long> CreateNotificationAsync(NotificationModel message);

        Task<NotificationModel> GetNotificationByIdAsync(long notificationId);

        Task<NotificationModel> UpdateNotificationAsync(long id, NotificationModel notification);

        Task<IEnumerable<UserNotificationModel>> GetAllNotificationAsync();

        Task DeleteNotificationAsync(long notificationId);

        Task<bool> SendNotificationToUserAsync(long notificationId, string userId);

        Task<bool> SendNotificationToUsersAsync(long notificationId, List<string> usersIds);

        Task<bool> SendNotificationToAllUsersAsync(long notificationId);

        Task<bool> MarkNotificationAsSentAsync(long notificationId);

        Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId);
    }
}
