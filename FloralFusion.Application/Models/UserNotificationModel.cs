using FloralFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloralFusion.Application.Models
{
    public class UserNotificationModel
    {
        public string UserId { get; set; }

        public long NotificationId { get; set; }

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
