using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using RoDuino.SMS.Bll.Notifications;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Components;
using RoDuino.SMS.Components.RoApplicationMenu;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Views.Config;
using res = RoDuino.SMS.Properties.Resources;


namespace RoDuino.SMS
{
    /// <summary>
    /// Interaction logic for RoExplorer.xaml
    /// </summary>
    public partial class RoExplorer 
    {
        private string title;
        private TabsElementFlow elementFlow;

        public RoExplorer()
        {
            InitializeComponent();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NotificationsExtensions.Notify += NotificationsExtensions_Notify;
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            title = res.ResourceManager.GetString("Application_Title") + " " + version;
            //debugLevel = ConfigurationManager.AppSettings[key];
//            RoConfig.Instance.DebugLevel
        }

        void NotificationsExtensions_Notify(string message, double percentage)
        {
            if (grdProgressBar.Visibility != Visibility.Visible)
                grdProgressBar.Visibility = Visibility.Visible;

            //Console.WriteLine("NOTIF:"+percentage+" "+message);
            this.txtMessage.Text = message;
            //if no percentage, 
            if (percentage != 0)
            {
                progressBar.IsIndeterminate = false;
                progressBar.Value = percentage;
            }
            else
            {
                progressBar.IsIndeterminate = true;
            }

            //fade out animation
            Storyboard s = (Storyboard)this.FindResource("showNotification");
            s.Begin(grdProgressBar);

            //force redisplay
            DoEvents();

            grdProgressBar.Visibility = Visibility.Collapsed;

        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

        /// <summary>
        /// the glass effect
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GlassHelper.ExtendGlassFrame(this, new Thickness(-1.0));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            
            foreach (Tab tab in Tabs.Instance.ItemTabs)
            {
                tabsGrid.Children.Add(tab);
            }
            //            lstTabs.ItemsSource = Tabs.Instance.ItemTabs;

            Tabs.Instance.ItemTabs.CollectionChanged += ItemTabs_CollectionChanged;

            Tabs.Instance.Current.Navigate("Login", "Login", null);


            // Get reference to ElementFlow
            DependencyObject obj = VisualTreeHelper.GetChild(vrAppList, 0);
            while ((obj is TabsElementFlow) == false)
            {
                obj = VisualTreeHelper.GetChild(obj, 0);
            }
            elementFlow = obj as TabsElementFlow;

            elementFlow.TabLoaded += elementFlow_TabLoaded;
            elementFlow.TabClosed += elementFlow_TabClosed;
            elementFlow.CurrentElementMouseEntered += ShowCloseButton;
            elementFlow.CurrentElementMouseLeaved += HideCloseButton;
            elementFlow.SelectedIndexChanged += elementFlow_SelectedIndexChanged;
            elementFlow.SelectedIndex = 0;
            vrAppList.ItemsSource = Tabs.Instance.ItemTabs;

            //init menu items 
//            menu.Items = (GroupList)VRSession.Instance["menus"];
//            menu.Focusable = false;
//            menu.MenuNavigated += vrMenu_MenuNavigated;
            //display menu
//            menu.Visibility = Tabs.Instance.Current.MenuVisible;
            //            scrollVisualGroup.Visibility = Tabs.Instance.Current.MenuVisible;
            grdAppList.Visibility = Tabs.Instance.Current.MenuVisible;
            ApplicationMenu.Visibility = Tabs.Instance.Current.MenuVisible;
            Tabs.Instance.Current.PropertyChanged += Current_PropertyChanged;
        }

        private void MoveNextApp(object sender, RoutedEventArgs e)
        {
            if (elementFlow.SelectedIndex < elementFlow.Children.Count)
                elementFlow.ChangeIndex(elementFlow.SelectedIndex + 1);
        }

        private void MovePrevApp(object sender, RoutedEventArgs e)
        {
            if (elementFlow.SelectedIndex > 0)
                elementFlow.ChangeIndex(elementFlow.SelectedIndex - 1);
        }

        private void ItemTabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Tab tab in e.NewItems)
                    tabsGrid.Children.Add(tab);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Tab tab in e.OldItems)
                    tabsGrid.Children.Remove(tab);
            }
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("View"))
            {
                //display menu
                ApplicationMenu.Visibility = Tabs.Instance.Current.MenuVisible;
                //                scrollVisualGroup.Visibility = Tabs.Instance.Current.MenuVisible;
                grdAppList.Visibility = Tabs.Instance.Current.MenuVisible;
            }
        }

        private void ShowTab(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.Current = ((TabButton)sender).Tab;
//            menu.Visibility = Tabs.Instance.Current.MenuVisible;
        }

        private void elementFlow_TabLoaded(Tab tab)
        {
            Tabs.Instance.Current = tab;
//            menu.Visibility = Tabs.Instance.Current.MenuVisible;
        }

        private void elementFlow_TabClosed(Tab tab)
        {
            Tabs.Instance.Close(tab);
        }

        private void HideCloseButton(Tab tab)
        {
            if (!btnCloseApp.IsMouseOver)
                btnCloseApp.Visibility = Visibility.Collapsed;
        }

        private void ShowCloseButton(Tab tab)
        {
            if (Tabs.Instance.ItemTabs.Count > 1)
                btnCloseApp.Visibility = Visibility.Visible;
        }

        private void elementFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMoveRight.Visibility = (elementFlow.SelectedIndex == elementFlow.Children.Count - 1)
                                          ? Visibility.Collapsed
                                          : Visibility.Visible;
            btnMoveLeft.Visibility = (elementFlow.SelectedIndex > 0)
                                         ? Visibility.Visible
                                         : Visibility.Collapsed;
            if (elementFlow.Children.Count <= 1)
            {
                btnMoveRight.Visibility = Visibility.Collapsed;
                btnMoveLeft.Visibility = Visibility.Collapsed;
            }
            else if (elementFlow.Children.Count > 3)
            {
                if (elementFlow.SelectedIndex == elementFlow.Children.Count - 1)
                    btnMoveRight.Visibility = Visibility.Visible;
                if (elementFlow.SelectedIndex == 0) btnMoveLeft.Visibility = Visibility.Visible;
            }
        }

        private void btnMoveLeft_Loaded(object sender, RoutedEventArgs e)
        {
            //take screenshot
            Grid grd = new Grid();
            grd.Background = (ImageBrush)this.FindResource("HeaderBrush");
            int height = 100;
            grd.Height = height;
            grd.Width = tabsGrid.ActualWidth;
            tabsGrid.Children.Add(grd);
            grd.Measure(new Size(tabsGrid.ActualWidth, height));
            Size size = grd.DesiredSize;
            grd.Arrange(new Rect(new Point(0, 0), size));
            byte[] img = TextureUtil.TakeScreenShot(tabsGrid, (int)tabsGrid.ActualWidth, (int)tabsGrid.ActualHeight);
            //crop image
            Point p = btnMoveLeft.TranslatePoint(new Point(0, 0), mainGrid);
            img = TextureUtil.CropImageFile(img, 80, height - 2, (int)p.X, 0);

            //create brush
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.StreamSource = new MemoryStream(img);
            bmImage.CreateOptions = BitmapCreateOptions.None;
            bmImage.CacheOption = BitmapCacheOption.Default;
            bmImage.EndInit();
            btnMoveLeft.Background = new ImageBrush(bmImage);
            tabsGrid.Children.Remove(grd);
        }

        private void btnMoveRight_Loaded(object sender, RoutedEventArgs e)
        {
            //take screenshot
            Grid grd = new Grid();
            grd.Background = (ImageBrush)this.FindResource("HeaderBrush");
            int height = 100;
            grd.Height = height;
            grd.Width = tabsGrid.ActualWidth;
            tabsGrid.Children.Add(grd);
            grd.Measure(new Size(tabsGrid.ActualWidth, height));
            Size size = grd.DesiredSize;
            grd.Arrange(new Rect(new Point(0, 0), size));
            byte[] img = TextureUtil.TakeScreenShot(tabsGrid, (int)tabsGrid.ActualWidth, (int)tabsGrid.ActualHeight);
            //crop image
            Point p = btnMoveRight.TranslatePoint(new Point(0, 0), mainGrid);
            img = TextureUtil.CropImageFile(img, 80, height - 2, (int)p.X, 0);

            //create brush
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.StreamSource = new MemoryStream(img);
            bmImage.CreateOptions = BitmapCreateOptions.None;
            bmImage.CacheOption = BitmapCacheOption.Default;
            bmImage.EndInit();
            btnMoveRight.Background = new ImageBrush(bmImage);
            tabsGrid.Children.Remove(grd);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            btnMoveRight_Loaded(sender, e);
            btnMoveLeft_Loaded(sender, e);
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            if (Tabs.Instance.ItemTabs.Count == 1)
                return;

            tabsGrid.Children.Remove(elementFlow.GetCurrentTab);
            Tabs.Instance.Close(elementFlow.GetCurrentTab);
            elementFlow.SelectIndexByTab(Tabs.Instance.Current);
        }



        private void btnMessages_Clicked(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.NewTab("Messages/Index.xaml", null, res.ResourceManager.GetString("Messages"));
        }

        private void btnContacts_Clicked(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.NewTab("Main/List.xaml", null, res.ResourceManager.GetString("Contacts"));
        }

        private void btnConfig_Clicked(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.NewTab("Config/Index.xaml", null, res.ResourceManager.GetString("Configurations"));            
        }

        private void btnHistory_Clicked(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.NewTab("History/Index.xaml", null, res.ResourceManager.GetString("History"));                        
        }

        private void btnImport_Clicked(object sender, RoutedEventArgs e)
        {
            Tabs.Instance.NewTab("Import/Index.xaml",null,res.ResourceManager.GetString("Import"));
        }
    }
}
