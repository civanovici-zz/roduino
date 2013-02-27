using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Components;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Helpers.Collections;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Views.Main
{
    /// <summary>
    /// Interaction logic for List.xaml
    /// </summary>
    public partial class List 
    {
        private ClientsCollection clients=new ClientsCollection();
        CollectionViewSource source = new CollectionViewSource();
        private bool isAllSelected;

        public List()
        {
            InitializeComponent();
            
            
            Canvas.SetZIndex(stackPanel1, 100);
        }



        #region Overrides of View


        /// <summary>
        ///  occurs after the page has been navigated to and data binded
        /// </summary>
        public override void Navigated()
        {
            btnSendSms.Click += btnSendSms_Click;
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnSelectAll.Click += btnSelectAll_Click;
            btnRefresh.Click += btnRefresh_Click;
//            (txtFilter).TextChanged += FilterTextBox_TextChanged;
        }

        /// <summary>
        ///  occurs right before navigating away from the page
        /// </summary>
        public override void Navigating()
        {
            btnSendSms.Click -= btnSendSms_Click;
            btnAdd.Click -= btnAdd_Click;
            btnEdit.Click -= btnEdit_Click;
            btnDelete.Click -= btnDelete_Click;
            btnSelectAll.Click -= btnSelectAll_Click;
            btnRefresh.Click -= btnRefresh_Click;
        }



        /// <summary>
        /// Databind
        /// </summary>
        public override void DataBind()
        {
            //            gridImages.View.UseDefaultHeadersFooters = false;
            clients.Clear();
            var cls = (IQueryable<Client>) PropertyBag["clients"];
            foreach (var client in cls)
            {
                clients.Add(client);
            }
            
            source.Source = clients;
            grdClients.ItemsSource = source.View;
        }

        #endregion
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(grdClients.SelectedItems.Count==0)
            {
                RoAlertBox alertBox = new RoAlertBox(res.ResourceManager.GetString("PleaseSelectFromList"));
                alertBox.ShowDialog();
                return;
            }
            RoConfirmationBox msg = new RoConfirmationBox(res.ResourceManager.GetString("AreYouSure"));
            var r=msg.ShowDialog();
            if (msg.DialogResult == false) return;

            IList<int> cls = new List<int>();
            foreach (var item in grdClients.SelectedItems)
            {
                cls.Add(((Client)item).Id);
            }
            PropertyBag["clients"] = cls.ToArray();
            IDictionary prop = NavigatorFactory.Navigator.Get("Main", "Delete", PropertyBag);
            if (prop.Contains("flash"))
            {
                Flash flash = (Flash)prop["flash"];
                ShowVRAlertBox(flash.Error);
            }
            clients.Clear();
            var cl= (IQueryable<Client>)prop["clients"];
            foreach (var client in cl)
            {
                clients.Add(client);
            }
            source.Source = null;
            source.Source = clients;
        }

        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdClients.SelectedItems.Count != 1)
            {
                RoAlertBox alertBox = new RoAlertBox(res.ResourceManager.GetString("PleaseSelectOnlyOne"));
                alertBox.ShowDialog();
                return;
            }
            PropertyBag["client"] = (Client)grdClients.SelectedItems[0];
            NavigatorFactory.Navigator.Navigate("Main/Edit.xaml", PropertyBag,true);
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigatorFactory.Navigator.Navigate("Main/Add.xaml",PropertyBag, true);
        }

        void btnSendSms_Click(object sender, RoutedEventArgs e)
        {
            if(grdClients.SelectedItems.Count==0)
            {
                RoAlertBox msg = new RoAlertBox(res.ResourceManager.GetString("PleaseSelectFromList"));
                msg.ShowDialog();
            }
            else
            {
                if(string.IsNullOrEmpty(txtMessage.Text))
                {
                    RoAlertBox msg = new RoAlertBox(res.ResourceManager.GetString("PleaseEnterMessage"));
                    msg.ShowDialog();
                    return;
                }
                IList<int> cls = new List<int>();
                foreach (var item in grdClients.SelectedItems)
                {
                    cls.Add(((Client)item).Id);
                }
                PropertyBag["clients"] = cls.ToArray();
                PropertyBag["message"] = txtMessage.Text;
                NavigatorFactory.Navigator.Navigate("Main/SendSMS.xaml", PropertyBag,true);
                
                //deselect all selection
                grdClients.UnselectAll();
                    
                
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            grdClients.FilterList(txtFilter.Text);
            if (grdClients.Items.Count > 0)
            {
                grdClients.ScrollIntoView(grdClients.Items[0]);
            }

        }

        void FilterTextBox_ResetFilter(object sender, RoutedEventArgs e)
        {
//            TagCloudControl.ClearSelection();
//            AgeDistributionControl.ClearSelection();
//            BirthdaysControl.ClearSelection();
//            GenderDistributionControl1.ClearSelection();
//            LivingDistributionControl1.ClearSelection();
//
//            surnameFilter = null;
//            ageFilter = null;
//            birthdateFilter = null;
//            livingFilter = null;
//            genderFilter = null;
            scrollToTop();

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if(isAllSelected)
            {
                btnSelectAll.Content= res.ResourceManager.GetString("UnselectAll");
                grdClients.UnselectAll();
            }
            else
            {
                btnSelectAll.Content = res.ResourceManager.GetString("SelectAll");
                grdClients.SelectAll();
            }

            isAllSelected = !isAllSelected;

        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            IDictionary prop = NavigatorFactory.Navigator.Get("Main", "List", new Hashtable());
            PropertyBag["clients"] = prop["clients"];
            DataBind();
        }


        private void scrollToTop()
        {
            try
            {
                grdClients.ScrollIntoView(grdClients.Items[0]);
            }
            catch { }
        }
    }
}
