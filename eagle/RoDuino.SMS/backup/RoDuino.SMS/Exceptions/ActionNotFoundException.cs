using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class ActionNotFoundException : Exception
    {
        public ActionNotFoundException(string message)
            : base(message)
        {
        }

        public ActionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ActionNotFoundException(string controller, string action, Exception innerException) :
            base(String.Format("Controller {0} does not contain an action {1}", controller, action), innerException)
        {
        }

        public ActionNotFoundException(string controller, string action) :
            base(String.Format("Controller {0} does not contain an action {1}", controller, action))
        {
        }
    }
}
