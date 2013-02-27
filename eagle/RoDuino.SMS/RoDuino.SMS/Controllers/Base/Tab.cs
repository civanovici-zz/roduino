using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RoDuino.SMS.Views;

namespace RoDuino.SMS.Controllers.Base
{
    public class Tab : Grid, INotifyPropertyChanged
    {
        private View view;

        #region "properties"

        private Brush visualBrush;
        public string TabName { get; set; }
        public byte[] Image { get; set; }

        public string Uri { get; set; }
        public Guid Id { get; set; }

        

        /// <summary>
        /// the visual brush is used in the tabs
        /// </summary>
        public Brush VisualBrush
        {
            get { return this.visualBrush; }
            set
            {
                //                this.visualBrush = null;
                //                return;
                //clear old
                if (this.visualBrush != null)
                {
                    visualBrush = null;
                }
                //set new
                this.visualBrush = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("VisualBrush"));
            }
        }

        /// <summary>
        /// the view inside the tab
        /// </summary>
        public View View
        {
            get { return view; }
            set
            {
                if (view != null)
                {
                    this.Children.Remove(view);
                    view.Content = null; //force remove reference (for memory leak)
                    view = null;
                }
                view = value;
                this.Children.Add(view);

                //the visual brush is used in the tabs
                if (HasVisualBrush)
                {
                    VisualBrush = new VisualBrush(this.View);
                }
                else
                    VisualBrush = this.View.Brush;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("View"));
            }
        }

        /// <summary>
        /// whether it should also display a menu
        /// </summary>
        public Visibility MenuVisible
        {
            get { return view != null ? view.MenuVisible : Visibility.Visible; }
        }

        /// <summary>
        /// whether should create a visual brush that is to be displayed in the tabs
        /// IMPORTANT:it is important that store doesn't create one as it slows down the store significantly
        /// </summary>
        public bool HasVisualBrush
        {
            get { return view != null ? view.HasVisualBrush : false; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// close from main tabs
        /// </summary>
        public void Close()
        {
            if (view != null)
            {
                view.Close();
                this.Children.Remove(view);
                view = null;
            }
        }

        /// <summary>
        /// make visible
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// make visibilit colapsed
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Navigate(string uri)
        {
            NavigatorFactory.Navigator.Navigate(uri);
        }

        public void Navigate(string uri, IDictionary args)
        {
            NavigatorFactory.Navigator.Navigate(uri, args);
        }

        public void Navigate(string controller, string action, Hashtable propertyBag)
        {
            NavigatorFactory.Navigator.Navigate(controller, action, propertyBag);
        }

        public void Navigate(string controller, string action)
        {
            NavigatorFactory.Navigator.Navigate(controller, action);
        }
    }
}
