using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class AmbiguousControllerNameException:Exception
    {
        public AmbiguousControllerNameException(string message)
            : base(message)
        {
        }

        public AmbiguousControllerNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
