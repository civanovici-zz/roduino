using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class FriendlyException
    {
        public FriendlyException()
        {
            FriendlyMessage = "";
            ActualMessage = "";
            ActualStackTrace = "";
        }
        public FriendlyException(string friendlyMessage, Exception exception)
        {
            this.FriendlyMessage = friendlyMessage;
            this.ActualException = exception;
        }


        public string FriendlyMessage
        { set; get; }
        public Exception ActualException
        { set; get; }
        /// <summary>
        /// the message of the exception
        /// </summary>
        public string ActualMessage
        { set; get; }
        public string ActualStackTrace
        { set; get; }
    }
}
