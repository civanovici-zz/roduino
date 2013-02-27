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
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Notifications;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Views.Config;
using RoDuino.SMS.Views.Login;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    /// <summary>
    /// Interaction logic for LoginGrid.xaml
    /// </summary>
    public partial class LoginGrid : System.Windows.Controls.Grid
    {
        public LoginGrid()
        {
            InitializeComponent();
#if (!DEBUG)
            {
                btnLoginAdmin.Visibility = Visibility.Collapsed;
            }
#endif
            this.txtPassword.KeyDown += ReturnKeyPressed;
            this.txtUserName.KeyDown += ReturnKeyPressed;
        }

        private void grdMain_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionData data = ((ConnectionData)((ListBoxItem)this.DataContext).Content);
            txtConnectionName.Text = data.FileName;
            txtUserName.Text = data.Username;
        }


        public string CurrentConnection { get; set; }

        private void btnLoginAdmin_Click(object sender, RoutedEventArgs e)
        {
            Hashtable PropertyBag = new Hashtable();
            PropertyBag["connection"] = GetConnectionName();
            this.RaiseNotification(res.ResourceManager.GetString("LoginCheckingCredentials"));
            //NavigatorFactory.Navigator.Get("Login", "AuthentificateAdmin", PropertyBag);
            //this.RaiseNotification(res.ResourceManager.GetString("LoginLoadingApplication"));
            //NavigatorFactory.Navigator.Navigate("Range", "List", PropertyBag);
            IDictionary newBag = NavigatorFactory.Navigator.Get("Login", "AuthentificateAdmin", PropertyBag);
            if (newBag.Contains("flash"))
            {
                Flash flash = (Flash)newBag["flash"];
                RoAlertBox msg = new RoAlertBox(flash.Error);
                msg.Topmost = true;
                msg.ShowDialog();
                //                ShowVRAlertBox(flash.Error);
            }
            else
            {

                this.RaiseNotification(res.ResourceManager.GetString("LoginLoadingApplication"));

                NavigatorFactory.Navigator.Navigate("Main", "List", PropertyBag);
//                NavigatorFactory.Navigator.Navigate("Config", "Index", PropertyBag);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
            //            NavigatorFactory.Navigator.Navigate("Login", "Authentificate", PropertyBag);
        }

        private void Login()
        {
            Hashtable PropertyBag = new Hashtable();
            User user = new User();
            user.Username = txtUserName.Text;
            user.Password = txtPassword.Password;
            PropertyBag["user"] = user;
            PropertyBag["connection"] = GetConnectionName();
            this.RaiseNotification(res.ResourceManager.GetString("LoginCheckingCredentials"));
            IDictionary newBag = NavigatorFactory.Navigator.Get("Login", "Authentificate", PropertyBag);
            if (newBag.Contains("flash"))
            {
                Flash flash = (Flash)newBag["flash"];
                RoAlertBox msg = new RoAlertBox(flash.Error);
                msg.Topmost = true;
                msg.ShowDialog();
                //                ShowVRAlertBox(flash.Error);
            }
            else
            {
                this.RaiseNotification(res.ResourceManager.GetString("LoginLoadingApplication"));
                NavigatorFactory.Navigator.Navigate("Main", "List", PropertyBag);
            }
        }

        private void ReturnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login();
            }
            if (e.Key == Key.Tab)
            {
                if (sender == txtUserName)
                {
                    txtPassword.Focus();
                    e.Handled = true;
                }
                if (sender == txtPassword)
                {
                    btnLogin.Focus();
                    e.Handled = true;
                }
            }
        }

        private string GetConnectionName()
        {
            if (string.IsNullOrEmpty(CurrentConnection))
            {
                ConnectionData data = ((ConnectionData)((ListBoxItem)this.DataContext).Content);
                return data.FileName;
            }
            else
            {
                return CurrentConnection;
            }
        }

    }
}
