using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RoDuino.SMS.Components.RoApplicationMenu;
using RoDuino.SMS.Components.RoApplicationMenu.ViewStates;
using RoDuino.SMS.Helpers;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Views.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        private IList connectionList;
        private LoginElemtFlow elementFlow;
        private string currentConnection;

        public Login()
        {
            InitializeComponent();
        }

        public override Visibility MenuVisible
        {
            get { return Visibility.Hidden; }
        }

        public override void DataBind()
        {
            connectionList = (IList)PropertyBag["connections"];
            currentConnection = (string)PropertyBag["connection"];
            if (PropertyBag["flash"] != null)
                ShowVRAlertBox(((Flash)PropertyBag["flash"]).Error);
            if (connectionList.Count == 0)
                ShowVRAlertBox(res.ResourceManager.GetString("Login_NoConnectionFileFound"));
            return;
        }

        public override void OnLoad(object sender, RoutedEventArgs e)
        {
            base.FadeOut();
            // Get reference to ElementFlow

            FindElementFlow();
            elementFlow.SelectedIndex = 0;
            VForm view = new VForm();
            elementFlow.CurrentView = view;
            vrLoginList.ItemsSource = connectionList;
            if (connectionList.Count > 0)
                ((VForm)elementFlow.CurrentView).LoginGrid.CurrentConnection = currentConnection;

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
//            txtVersion.Text = "Version " + version;
        }

        private void FindElementFlow()
        {
            DependencyObject obj = VisualTreeHelper.GetChild(vrLoginList, 0);
            while ((obj is ElementFlow) == false)
            {
                obj = VisualTreeHelper.GetChild(obj, 0);
            }
            elementFlow = obj as LoginElemtFlow;
        }



    }
}
