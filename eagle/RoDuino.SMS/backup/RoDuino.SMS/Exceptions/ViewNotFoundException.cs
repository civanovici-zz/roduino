using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string message)
            : base(message)
        {
        }

        public ViewNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
