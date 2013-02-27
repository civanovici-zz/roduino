using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Exceptions
{
    public class AmbiguousActionNameException:Exception
    {
        public AmbiguousActionNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AmbiguousActionNameException(string controller, string action, Exception innerException)
            : base(
                String.Format("Controller {0} contains more actions with name {1}. Which one to invoke?", controller,
                              action), innerException)
        {
        }
    }
}
