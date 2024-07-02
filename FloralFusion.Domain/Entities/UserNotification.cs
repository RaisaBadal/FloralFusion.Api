using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{

    [Table("UserNotifications")]
    public class UserNotification :AbstractEntity
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Notification))]
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
