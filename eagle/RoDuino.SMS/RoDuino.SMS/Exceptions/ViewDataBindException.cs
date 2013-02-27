using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class ViewDataBindException:Exception
    {
        public ViewDataBindException(string message)
            : base(message)
        {
        }

        public ViewDataBindException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
