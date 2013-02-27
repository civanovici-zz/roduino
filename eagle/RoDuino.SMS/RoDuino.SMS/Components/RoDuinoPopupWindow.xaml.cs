using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;

namespace RoDuino.SMS.Components
{
    /// <summary>
    /// Interaction logic for RoDuinoPopupWindow.xaml
    /// </summary>
    public partial class RoDuinoPopupWindow : Window
    {
        public RoDuinoPopupWindow(string uri, IDictionary propertyBag)
        {
            InitializeComponent();
            NavigatorFactory.Navigator.Load(null, frameContent, uri, propertyBag);
            //            NavigatorFactory.Navigator.Navigate(uri);
            SizeChanged += PopupWindow_SizeChanged;
            LocationChanged += PopupWindow_LocationChanged;
            //            Uri imageUri = new Uri(@"Content/close_up.png", UriKind.Relative);
            //            Image image =new Image();
            //            image.Source = new BitmapImage(imageUri);
            //            image.Height = 15;
            //            image.Width= 15;
            //            close.Content = image;
            //            Debug.WriteLine("Img:"+image.Source.ToString());
        }

        public RoDuinoPopupWindow()
        {
            InitializeComponent();
        }

        private void PopupWindow_LocationChanged(object sender, EventArgs e)
        {
            Rect workArea = SystemParameters.WorkArea;
            this.Left = (workArea.Width - this.ActualWidth) / 2;
            this.Top = (workArea.Height - this.ActualHeight) / 2;
        }

        private void PopupWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rect workArea = SystemParameters.WorkArea;
            this.Left = (workArea.Width - this.ActualWidth) / 2;
            this.Top = (workArea.Height - this.ActualHeight) / 2;
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void UpdateTitle(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GlassHelper.ExtendGlassFrame(this, new Thickness(-1.0));
        }

        protected string GetUri(string uri)
        {
            return uri.Split(',')[1];
        }

        public void ParseSettings(string tag)
        {
            Hashtable args = new Hashtable();
            string paramsDictionary = tag.Substring(tag.IndexOf("?") + 1);
            if (paramsDictionary.Length == 0) return;
            string[] param = paramsDictionary.Split(';');
            foreach (string s in param)
            {
                if (s.IndexOf('=') > 0)
                {
                    args[s.Split('=')[0]] = s.Split('=')[1];
                }
            }

            if (args["ResizeMode"] != null)
            {
                this.ResizeMode = ResizeMode.NoResize;
                //                mainBorder.Background=
            }
            if (args["Height"] != null)
            {
                this.Height = double.Parse((string)args["Height"]);
            }
            if (args["Width"] != null)
            {
                this.Width = double.Parse((string)args["Width"]);
            }
            if (args["Title"] != null)
            {
                this.Title = (string)args["Title"];
            }
        }
    }
}
