using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Views.Main
{
    /// <summary>
    /// Interaction logic for SendSms.xaml
    /// </summary>
    public partial class SendSms 
    {
        private bool continueSending;
        private IList<SmsHistory> list = new List<SmsHistory>();
        CollectionViewSource source = new CollectionViewSource();

        public SendSms()
        {
            InitializeComponent();
        }

        public override void DataBind()
        {
            list = (IList<SmsHistory>)PropertyBag["histories"];
            source.Source = list;
            grdClients.ItemsSource = source.View;
        }

        public override void Navigated()
        {
            btnStopQueue.Click += btnStopQueue_Click;
        }

        public override void Navigating()
        {
            btnStopQueue.Click -= btnStopQueue_Click;
        }

        void btnStopQueue_Click(object sender, RoutedEventArgs e)
        {
            if(continueSending)
            {
                continueSending = false;
                btnStopQueue.Content = res.ResourceManager.GetString("Resume");
//                btnStopQueue.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new SendSmsDelegate(SendSmsWorker));
            }
            else
            {
                btnStopQueue.Content = res.ResourceManager.GetString("StopCurrentQueue");
                continueSending = true;
//                StartSendingSMS();
                btnStopQueue.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new SendSmsDelegate(SendSmsWorker));
            }
        }
        private delegate void SendSmsDelegate();

        private int currentItem=0;
        private int smsErros = 0;

        public void SendSmsWorker()
        {
            try
            {
                if (currentItem >= grdClients.Items.Count)
                {
                    btnStopQueue.IsEnabled = false;
                    continueSending = false;
                    btnStopQueue.Content = res.ResourceManager.GetString("Complete");

                    string message = "Total SMS " + grdClients.Items.Count + Environment.NewLine + "Complete: " +
                                     (grdClients.Items.Count - smsErros) + Environment.NewLine + "Erori: " + smsErros;
                    ShowVRAlertBox(message);
                    return;
                }
                //send sms
                PropertyBag["history"] = grdClients.Items[currentItem];
                IDictionary prop = NavigatorFactory.Navigator.Get("Main", "TrySendSMS", PropertyBag);
                if (prop.Contains("flash"))
                {
                    Flash flash = (Flash)prop["flash"];
                    ShowVRAlertBox(flash.Error);
                }
                ((SmsHistory)grdClients.Items[currentItem]).Status = ((SmsHistory)prop["history"]).Status;

                if (((SmsHistory)prop["history"]).HasError) smsErros++;

                grdClients.Items.Refresh();
                source.DeferRefresh();
                //increment current
                currentItem++;

                Thread.Sleep(100);
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
            }
            if (continueSending)
            {
                btnStopQueue.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new SendSmsDelegate(SendSmsWorker));
            }

//            for (int i = 0; i <= 5000; i++)
//            {
//                Console.WriteLine("Begin filter");
//                Thread.Sleep(5);
//            }

        }
    }
}
