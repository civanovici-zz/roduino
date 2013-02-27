using System;
using System.Collections;
using System.Windows;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Views.Main
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit 
    {
        private Client client;

        public Edit()
        {
            InitializeComponent();
        }

        public override void DataBind()
        {
            client = (Client) PropertyBag["client"];
            txtName.Text = client.Name;
            txtPhone.Text = client.Phone;
            txtNetwork.Text = client.Network;
        }

        public override void Navigated()
        {
            btnSave.Click += btnSave_Click;
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = "";
            if(string.IsNullOrEmpty(txtName.Text))
                errorMsg += res.ResourceManager.GetString("PleaseEnterName")+Environment.NewLine;
            if(string.IsNullOrEmpty(txtPhone.Text))
                errorMsg += res.ResourceManager.GetString("PleaseEnterPhone")+Environment.NewLine;
            if(string.IsNullOrEmpty(txtNetwork.Text))
                errorMsg += res.ResourceManager.GetString("PleaseEnterNetwork")+Environment.NewLine;
            if(errorMsg=="")
            {
                client.Name = txtName.Text;
                client.Phone = txtPhone.Text;
                client.Network = txtNetwork.Text;
                PropertyBag["client"] = client;
                IDictionary prop = NavigatorFactory.Navigator.Get("Main", "Save", PropertyBag);
                if (prop.Contains("flash"))
                {
                    Flash flash = (Flash)prop["flash"];
                    ShowVRAlertBox(flash.Error);
                }
                else
                {
//                    this.Close();
//                    NavigatorFactory.Navigator.Redirect("Main", "List", PropertyBag);
                }
            }
            else
            {
                ShowVRAlertBox(errorMsg);
            }
        }

        public override void Navigating()
        {
            btnSave.Click -= btnSave_Click;            
        }
    }
}
