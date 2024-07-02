using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace FloralFusion.Domain.Entities
{
    [Table("Notifications")]
    public class Notification:AbstractEntity
    {
        [StringLength(2000, ErrorMessage = "Such message is not valid", MinimumLength = 2)]
        public required string Message { get; set; }

        public long NotificationTypeId { get; set; }

        public DateTime CreatedAt { get; set; }=DateTime.Now;

        public bool IsActive { get; set; } = true;

        public required NotificationType NotificationType { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
