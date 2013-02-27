using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class FrameNotFoundException : Exception
    {
        public FrameNotFoundException(string message)
            : base(message)
        {
        }

        public FrameNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
