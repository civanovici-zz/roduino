using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Controllers.Base
{
    public class Tabs
    {
        private static Tabs instance;
        private Tab current;

        public Tab Current
        {
            get { return current; }
            set { current = value; }
        }

        public static Tabs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Tabs();
                }
                return instance;
            }
            set { instance = value; }
        }
    }
}
