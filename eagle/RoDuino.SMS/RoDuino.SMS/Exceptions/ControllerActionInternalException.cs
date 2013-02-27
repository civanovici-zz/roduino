using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class ControllerActionInternalException : Exception
    {
        public ControllerActionInternalException(string message)
            : base(message)
        {
        }

        public ControllerActionInternalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
