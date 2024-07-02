using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.Notification;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Application.Services.NotificationServices
{
    public class NotificationsService : AbstractClass, INotificationService
    {
        public readonly UserManager<User> userManager;
        public NotificationsService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService, UserManager<User> userManager) : base(uniteOfWork, mapper, smtpService)
        {
            this.userManager = userManager;
        }

        #region CreateNotificationAsync

        public async Task<long> CreateNotificationAsync(NotificationModel message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(message, nameof(message));
                var mapped = mapper.Map<Notification>(message)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                var res = await uniteOfWork.Notifications.Create(mapped);
                return res;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }

        }
        #endregion

        #region DeleteNotificationAsync
        public async Task DeleteNotificationAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                await uniteOfWork.Notifications.DeleteByIdAsync(notificationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion

        #region GetAllNotificationAsync
        public async Task<IEnumerable<UserNotificationModel>> GetAllNotificationAsync()
        {
            try
            {
                var res = await uniteOfWork.Notifications.GetAllNotificationAsync();
                if (!res.Any()) throw new GeneralException(ErrorKeys.NotFound);
                var mapped = mapper.Map<IEnumerable<UserNotificationModel>>(res)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                return mapped;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion

        #region GetNotificationByIdAsync

        public async Task<NotificationModel> GetNotificationByIdAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var res = await uniteOfWork.Notifications.GetByIdAsync(notificationId);
                var mapped= mapper.Map<NotificationModel>(res) ??
                    throw new GeneralException(ErrorKeys.Mapped);
                return mapped;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region GetUserNotificationsAsync

        public async Task<IEnumerable<NotificationModel>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var user = await userManager.FindByIdAsync(userId);
                if (user == null) throw new UserNotFoundException("There is no current user!");
                var notification = await GetAllNotificationAsync();
                var userNotification = notification.Where(i => i.UserId == userId);
                if (!userNotification.Any()) throw new NotificationNotFoundException($"No notification found for user: {userId}");
                var mapped = mapper.Map<IEnumerable<NotificationModel>>(userNotification)
                    ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion

        #region MarkNotificationAsSentAsync

        public async Task<bool> MarkNotificationAsSentAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var res= await uniteOfWork.Notifications.GetAllNotificationAsync();
                var userRes = res.Where(i => i.NotificationId == notificationId && i.SentDate is not null);
                if (!userRes.Any()) throw new GeneralException(ErrorKeys.NotFound);
                foreach (var item in userRes)
                {
                    item.IsSent = true;
                    await uniteOfWork.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region SendNotificationToAllUsersAsync
        public async Task<bool> SendNotificationToAllUsersAsync(long notificationId)
        {
            try
            {
                if (notificationId < 0) throw new GeneralException(ErrorKeys.BadRequest);
                string name = " ";
                string surname = " ";
                var notification = await GetNotificationByIdAsync(notificationId);
                if (notification == null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");

                var body = $@"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                }}
                .header {{
                    background-color: #007BFF;
                    color: #ffffff;
                    padding: 10px;
                    border-radius: 4px 4px 0 0;
                    text-align: center;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaaaaa;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>FloralFusion Notification</h1>
                </div>
                <div class=""content"">
                    <p>Dear {name + " " + surname},</p>
                    <p>{notification.Message}</p>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,</p>
                    <p>The FloralFusion Team</p>
                </div>
        </div>
    </body>
</html>";
                var users = await userManager.Users.ToListAsync();
                foreach (var item in users)
                {
                    name = item.Name;
                    surname = item.Surname;
                    smtpService.SendMessage(item.Email, "FloralFusion Notification", body);
                    var notificationUser = new UserNotificationModel()
                    {
                        SentDate = DateTime.Now,
                        NotificationId = notificationId,
                        UserId = item.Id
                    };
                    var mapped = mapper.Map<UserNotification>(notificationUser);
                    await uniteOfWork.Notifications.AttachNotificationToUserAsync(mapped);
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion

        #region SendNotificationToUserAsync
        public async Task<bool> SendNotificationToUserAsync(long notificationId, string userId)
        {

            if (notificationId < 0) throw new GeneralException(ErrorKeys.BadRequest);
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification == null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new UserNotFoundException(ErrorKeys.NotFound);
 
            var body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 20px;
                background-color: #f4f4f9;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                padding: 20px;
                border: 1px solid #dddddd;
                border-radius: 4px;
            }}
            .header {{
                background-color: #007BFF;
                color: #ffffff;
                padding: 10px;
                border-radius: 4px 4px 0 0;
                text-align: center;
            }}
            .content {{
                padding: 20px;
            }}
            .content p {{
                margin: 0 0 10px;
            }}
            .footer {{
                padding: 10px;
                text-align: center;
                font-size: 12px;
                color: #aaaaaa;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <div class=""header"">
                <h1>FloralFusion Notification</h1>
            </div>
            <div class=""content"">
                <p>Dear {user.Name},</p>
                <p>{notification.Message}</p>
                <p>If you have any questions, feel free to contact us.</p>
                <p>Best regards,</p>
                <p>The FloralFusion Team</p>
            </div>
            <div class=""footer"">
                &copy; {DateTime.Now.Year} FloralFusion. All rights reserved.
            </div>
        </div>
    </body>
    </html>";
            if (user.Email == null) throw new ArgumentException(ErrorKeys.BadRequest);
            smtpService.SendMessage(user.Email, "FloralFusion Notification", body);
            var userNotification = new UserNotificationModel
            {
                UserId = userId,
                NotificationId = notificationId,
                SentDate = DateTime.Now
            };
            var mapped = mapper.Map<UserNotification>(userNotification)
                ?? throw new GeneralException(ErrorKeys.Mapped);
            await uniteOfWork.Notifications.AttachNotificationToUserAsync (mapped);
            await uniteOfWork.SaveChanges();
            return true;
        }
        #endregion

        #region SendNotificationToUsersAsync

        public async Task<bool> SendNotificationToUsersAsync(long notificationId, List<string> usersIds)
        {
            try
            {
                if (notificationId < 0) throw new GeneralException(ErrorKeys.BadRequest);
                var notification = await GetNotificationByIdAsync(notificationId);
                if (notification == null) throw new NotificationNotFoundException($"No notification found by id: {notificationId}");
                string name = " ";
                string surname = " ";
                var body = $@"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                }}
                .header {{
                    background-color: #007BFF;
                    color: #ffffff;
                    padding: 10px;
                    border-radius: 4px 4px 0 0;
                    text-align: center;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 0 0 10px;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaaaaa;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>FloralFusion Notification</h1>
                </div>
                <div class=""content"">
                    <p>Dear {name + " " + surname},</p>
                    <p>{notification.Message}</p>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,</p>
                    <p>The FloralFusion Team</p>
                </div>
            <div class=""footer"">
                <p>&copy; 2024 The FlowerFusion. All rights reserved.</p>
            </div>
        </div>
    </body>
</html>";
                foreach (var item in usersIds)
                {
                    var user = await userManager.FindByIdAsync(item);
                    if (user is { Email: not null, Name: not null, Surname: not null })
                    {
                        name = user.Name;
                        surname = user.Surname;
                        smtpService.SendMessage(user.Email, $"FloralFusion Notification {DateTime.Now.ToShortTimeString()}", body);
                        var notificationUser = new UserNotificationModel
                        {
                            SentDate = DateTime.Now,
                            NotificationId = notificationId,
                            UserId = user.Id
                        };
                        var mapped = mapper.Map<UserNotification>(notificationUser);
                        await uniteOfWork.Notifications.AttachNotificationToUserAsync(mapped);
                        await uniteOfWork.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region UpdateNotificationAsync
        public async Task<NotificationModel> UpdateNotificationAsync(long id, NotificationModel notification)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(notification, nameof(notification));
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var map = mapper.Map<Notification>(notification);
                var res = await uniteOfWork.Notifications.Update(id, map);
                if (!res) throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = mapper.Map<NotificationModel>(notification)
                    ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
