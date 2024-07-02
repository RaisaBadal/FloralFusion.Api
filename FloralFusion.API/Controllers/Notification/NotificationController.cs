using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.Notification;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FloralFusion.API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="MANAGER")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notifications;
        private readonly IMemoryCache memoryCache;

        public NotificationController(INotificationService notifications, IMemoryCache memoryCache)
        {
            this.notifications = notifications;
            this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Response<long>> AddMessage(NotificationModel message)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var res = await notifications.CreateNotificationAsync(message);
            return res < 0 ? Response<long>.Error(ErrorKeys.UnSuccessFullInsert)
                : Response<long>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        public async Task<Response<NotificationModel>> GetNotificationById([FromRoute] long notificationId)
        {
            var cacheKey = $"Notification with id:{notificationId}";
            if (memoryCache.TryGetValue(cacheKey, out NotificationModel? notification) && notification is not null)
                return Response<NotificationModel>.Ok(notification);

            var res = await notifications.GetNotificationByIdAsync(notificationId);
            if (res is null) return Response<NotificationModel>.Error(ErrorKeys.NotFound);

            memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<NotificationModel>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        public async Task<Response<NotificationModel>> UpdateNotification([FromRoute] long id, [FromBody] NotificationModel notification)
        {
            if (!ModelState.IsValid || notification is null) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
            var res = await notifications.UpdateNotificationAsync(id, notification);
            if (res is null) return Response<NotificationModel>.Error(ErrorKeys.BadRequest);
            return Response<NotificationModel>.Ok(res);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<UserNotificationModel>>> AllNotification()
        {
            const string cacheKey = "AllNotification";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<UserNotificationModel>? result) && result is not null)
                return Response<IEnumerable<UserNotificationModel>>.Ok(result);

            var res = await notifications.GetAllNotificationAsync();
            if (!res.Any()) return Response<IEnumerable<UserNotificationModel>>.Error(ErrorKeys.BadRequest);

            memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<UserNotificationModel>>.Ok(res);
        }

        [HttpDelete]
        [Route("[action]/{notificationId}")]
        public async Task DeleteNotification([FromRoute] long notificationId)
        {
            await notifications.DeleteNotificationAsync(notificationId);
        }

        [HttpPost]
        [Route("[action]/{notificationId}/{userId}")]
        public async Task<Response<bool>> SendNotificationToUser([FromRoute] long notificationId, [FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.ArgumentNull);

            var res = await notifications.SendNotificationToUserAsync(notificationId, userId);

            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        public async Task<Response<bool>> SendNotificationToUsers([FromRoute] long notificationId, [FromBody] List<string> usersIds)
        {
            var res = await notifications.SendNotificationToUsersAsync(notificationId, usersIds);

            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPost]
        [Route("[action]/{notificationId}")]
        public async Task<Response<bool>> SendNotificationToAllUsers([FromRoute] long notificationId)
        {
            var res = await notifications.SendNotificationToAllUsersAsync(notificationId).ConfigureAwait(false);
            return res ? Response<bool>.Ok(res)
               : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        [HttpPut]
        [Route("[action]/{notificationId}")]
        public async Task<Response<bool>> MarkNotificationAsSent(long notificationId)
        {
            var res = await notifications.MarkNotificationAsSentAsync(notificationId).ConfigureAwait(false);
            return res ? Response<bool>.Ok(res)
              : Response<bool>.Error(ErrorKeys.BadRequest);
        }

        //davabrunot shesuli useris yvela message
        [Authorize(Roles ="CUSTOMER")]
        [HttpPost]
        [Route("[action]/{userId}")]
        public async Task<Response<IEnumerable<NotificationModel>>> GetUserNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.ArgumentNull);

            var cacheKey = $"Notifications of user:{userId}";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<NotificationModel>? notification) && notification is not null)
                return Response<IEnumerable<NotificationModel>>.Ok(notification);

            var res = await notifications.GetUserNotificationsAsync(userId);
            if (res is null) return Response<IEnumerable<NotificationModel>>.Error(ErrorKeys.NotFound);

            memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<NotificationModel>>.Ok(res);
        }
    }
}
