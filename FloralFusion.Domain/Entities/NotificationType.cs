using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Domain.Entities
{
    [Table("NotificationTypes")]
    [Index(nameof(Type))]
    public class NotificationType:AbstractEntity
    {
        public required string Type { get; set; }
        public IEnumerable<Notification>Notifications { get; set; }
    }
}
