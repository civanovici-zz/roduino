using System.Collections;
using System.Windows.Controls;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Views;

namespace RoDuino.SMS.Controllers.Base
{
    public class ARSessionByRequestNavigator : Navigator
    {
        /// <summary>
        /// it will initialize and close the AR session scope
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="args"></param>
        [ARSessionByRequest]
        public override void Navigate(string controllerName, string actionName, IDictionary args)
        {
            //navigate
            base.Navigate(controllerName, actionName, args);
        }


        /// <summary>
        /// it will initialize and close the AR session scope
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="args"></param>
        [ARSessionByRequest]
        public override void Redirect(string controllerName, string actionName, IDictionary args)
        {
            base.Redirect(controllerName, actionName, args);
        }

        /// <summary>
        /// it will initialize and close the AR session scope
        /// </summary>
        /// <param name="control"></param>
        /// <param name="uri"></param>
        /// <param name="args"></param>
        [ARSessionByRequest]
        public override void Load(View parentView, ContentControl control, string uri, IDictionary args)
        {
            base.Load(parentView, control, uri, args);
        }

        /// <summary>
        /// it will initialize and close the AR session scope
        /// </summary>
        [ARSessionByRequest]
        public override IDictionary Get(string controller, string action, IDictionary args)
        {
            return base.Get(controller, action, args);
        }
    }
}
