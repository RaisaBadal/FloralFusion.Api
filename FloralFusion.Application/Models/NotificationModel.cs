using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models
{
    public class NotificationModel
    {
        [StringLength(2000, ErrorMessage = "Such message is not valid", MinimumLength = 2)]
        public required string Message { get; set; }

        public long NotificationTypeId { get; set; }
    }
}
