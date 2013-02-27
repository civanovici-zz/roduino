using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class ControllerNotFoundException:Exception
    {
        public ControllerNotFoundException(string message)
            : base(message)
        {
        }

        public ControllerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
