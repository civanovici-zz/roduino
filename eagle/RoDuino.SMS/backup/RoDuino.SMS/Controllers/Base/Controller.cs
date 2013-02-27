using System.Collections;

namespace RoDuino.SMS.Controllers.Base
{
    public class Controller
    {
        private Hashtable propertyBag = new Hashtable();
        private string viewToRender = "";


        public Hashtable PropertyBag
        {
            get { return propertyBag; }
            set { propertyBag = value; }
        }


        public string ViewToRender
        {
            get { return viewToRender; }
            set { viewToRender = value; }
        }

        protected void RenderView(string viewName)
        {
            this.viewToRender = viewName;
        }

        protected void Redirect(string controllerName, string actionName)
        {
            NavigatorFactory.Navigator.Redirect(controllerName, actionName, this.propertyBag);
        }
    }
}
