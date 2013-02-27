using System.Collections;

namespace RoDuino.SMS.Bll
{
    class RoSession
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
