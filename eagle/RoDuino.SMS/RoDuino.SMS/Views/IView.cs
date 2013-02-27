using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RoDuino.SMS.Views
{
    public interface IView
    {
        Hashtable PropertyBag { set; get; }

        /// <summary>
        /// to hold all views that are loadede in content controls (AJAX like)
        /// </summary>
        List<IView> Views { set; get; }


        /// <summary>
        /// whether when this view is shown the menu should be visible
        /// </summary>
        Visibility MenuVisible { get; }

        /// <summary>
        /// whether should create a visual brush that is to be displayed in the tabs
        /// IMPORTANT:it is important that store doesn't create one as it slows down the store significantly
        /// </summary>
        bool HasVisualBrush { get; set; }

        /// <summary>
        /// executed on entry on the view after navigation. after it the propertybag is emptied
        /// </summary>
        void DataBind();

        /// <summary>
        /// occurs after the page has been navigated to and data binded
        /// </summary>
        void Navigated();

        /// <summary>
        /// occurs right before navigating away from the page
        /// </summary>
        void Navigating();

        /// <summary>
        /// executed when a tab containing a view is closed
        /// </summary>
        void Close();
    }
}
