namespace RoDuino.SMS.Controllers.Base
{
    public class NavigatorFactory
    {
        private static INavigator navigator;

        public static INavigator Navigator
        {
            get
            {
                if (navigator == null)
                {
                    navigator = new ARSessionByRequestNavigator();
                    //                    navigator = new ARReadOnlySessionNavigator();
                }
                return navigator;
            }
            set { navigator = value; }
        }
    }
}
