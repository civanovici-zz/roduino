using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Bll.Notifications
{
    /// <summary>
    /// used to add notification events to any class
    /// </summary>
    public static class NotificationsExtensions
    {

        public delegate void Notified(string message, double percent);
        public static event Notified Notify;

        /// <summary>
        /// raises a notification message
        /// that can be displayed by the UI
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void RaiseNotification(this object obj, string message)
        {
            if (Notify != null)
                Notify(message, 0);
        }


        /// <summary>
        /// raises a notification message
        /// that can be displayed by the UI
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <param name="message">to be used with a progress bar</param>
        /// <param name="percentage"></param>
        public static void RaiseNotification(this object obj, string message, double percentage)
        {
            if (Notify != null)
                Notify(message, percentage);
        }
    }
}
