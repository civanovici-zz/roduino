using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Framework;
using RoDuino.SMS.Bll.Util;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Exceptions
{
    public class ApplicationExceptionLogger
    {


        /// <summary>
        /// transforms the exceptions into friendly eaxecptions that are easier to display
        /// and to log
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static FriendlyException GetFriendlyException(Exception exception)
        {
            FriendlyException ex = null;
            if (exception is ControllerActionInternalException)
            {
                ex = new FriendlyException()
                {

                    FriendlyMessage = String.IsNullOrEmpty(RoLog.Instance.LastMessage)
                                          ? exception.Message
                                          : res.ResourceManager.GetString(RoLog.Instance.LastMessage),
                    ActualMessage = exception.InnerException.Message,
                    ActualStackTrace = exception.InnerException.StackTrace,
                    ActualException = exception.InnerException
                };

            }
            else if (exception is ViewDataBindException)
            {
                ex = new FriendlyException()
                {

                    FriendlyMessage = String.IsNullOrEmpty(RoLog.Instance.LastMessage)
                                          ? exception.Message
                                          : res.ResourceManager.GetString(RoLog.Instance.LastMessage),
                    ActualMessage = exception.InnerException.Message,
                    ActualStackTrace = exception.InnerException.StackTrace,
                    ActualException = exception.InnerException
                };

            }
            else if (exception is ActiveRecordException)
            {
                ex = new FriendlyException()
                {

                    FriendlyMessage = String.IsNullOrEmpty(RoLog.Instance.LastMessage)
                                          ? "There was an error trying to access the database.\n\n" + exception.Message
                                          : res.ResourceManager.GetString(RoLog.Instance.LastMessage),
                    ActualMessage = exception.Message,
                    ActualStackTrace = exception.StackTrace,
                    ActualException = exception
                };
            }
            else if (exception is IOException)
            {
                ex = new FriendlyException()
                {

                    FriendlyMessage = String.IsNullOrEmpty(RoLog.Instance.LastMessage)
                                          ? "There was an error trying to read or write from a file on the local computer."
                                          : res.ResourceManager.GetString(RoLog.Instance.LastMessage),
                    ActualMessage = exception.Message,
                    ActualStackTrace = exception.StackTrace,
                    ActualException = exception
                };
            }
            else
            {
                ex = new FriendlyException()
                {

                    FriendlyMessage = String.IsNullOrEmpty(RoLog.Instance.LastMessage)
                                          ? exception.Message
                                          : res.ResourceManager.GetString(RoLog.Instance.LastMessage),
                    ActualMessage = exception.Message,
                    ActualStackTrace = exception.StackTrace,
                    ActualException = exception
                };
            }
            return ex;
        }

        public static bool IsTestMode { set; get; }

    }
}
