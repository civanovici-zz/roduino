using RoDuino.SMS.Bll.Attributes;

namespace RoDuino.SMS.Bll
{
    public class RoConfig
    {
        private static RoConfig instance;
        private int debugLevel = TracedAttribute.DEBUG;


        private RoConfig()
        {
        }

        public static RoConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoConfig();
                }
                return instance;
            }
        }

        public int DebugLevel
        {
            get { return debugLevel; }
            set { debugLevel = value; }
        }
    }
}
