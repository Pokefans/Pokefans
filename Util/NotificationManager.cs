using System;
using Pokefans.Data;
using Pokefans.Data.UserData;

namespace Pokefans.Util
{
    public class NotificationManager
    {
        Entities db;

        public NotificationManager(Entities ents)
        {
            db = ents;
        }

        public void SendNotification(User user, string message)
        {
            SendNotification(user.Id, message);
        }

        public void SendNotification(User user, string message, string title) {
            SendNotification(user.Id, message, title);
        }

        public void SendNotification(int userid, string message)
        {
            SendNotification(userid, message, "<i class=\"fa fa-2x fa-newspaper-o\"></i>");
        }

        public void SendNotification(int userid, string message, string title)
        {
            UserNotification notification = new UserNotification();

            notification.Message = message;
            notification.Sent = DateTime.Now;
            notification.IsUnread = true;
            notification.UserId = userid;
            notification.Icon = title;

            db.UserNotifications.Add(notification);

            db.SaveChanges();
        }
    }
}
