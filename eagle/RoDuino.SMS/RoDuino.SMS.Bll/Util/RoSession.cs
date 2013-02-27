using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Bll.Util
{
    public class RoSession
    {
        private static RoSession instance;
        private Hashtable session;

        private RoSession()
        {
            if (session == null)
                session = new Hashtable();
        }

        public static RoSession Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoSession();
                }
                return instance;
            }
        }

        public object this[object key]
        {
            get { return session[key]; }
            set { session[key] = value; }
        }
    }
}
