using System.Collections;
using System.Windows.Controls;
using System.Windows.Navigation;
using RoDuino.SMS.Components;
using RoDuino.SMS.Views;

namespace RoDuino.SMS.Controllers.Base
{
    public interface INavigator
    {
        NavigationService NavigationService { set; get; }

        RoDuinoPopupWindow RoDuinoPopupWindow { get; set; }
        string Start(string controller, string action, IDictionary args);
        string Start(string cont, string action);

        void Redirect(string controller, string action, IDictionary args);

        void Navigate(string controller, string action, IDictionary args);
        void Navigate(string uri, IDictionary args, bool openNewPage);
        void Navigate(string controller, string action);
        void Navigate(string uri);
        void Navigate(string uri, IDictionary args);

        /// makes the roundtrip to controller
        /// BUT does not rerender the view, just gets the data back
        IDictionary Get(string controller, string action, IDictionary propertyBag);

        /// <summary>
        /// loads in a control a partial content
        /// </summary>
        /// <param name="parentView"></param>
        /// <param name="control"></param>
        /// <param name="uri"></param>
        /// <param name="args"></param>
        void Load(View parentView, ContentControl control, string uri, IDictionary args);

    }
}
