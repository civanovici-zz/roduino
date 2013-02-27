using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Threading;
using Castle.ActiveRecord;
using Microsoft.Win32;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Views.Import
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index 
    {
        private ObservableCollection<Client> list = new ObservableCollection<Client>();
        CollectionViewSource source = new CollectionViewSource();
        private delegate void ImportClientsDelegate();
        List<string> lines=new List<string>();
        private int currentLine = 0;
        private int imported;

        public Index()
        {
            InitializeComponent();
        }

        public override void DataBind()
        {
//            list = (ObservableCollection<Client>)PropertyBag["clients"];
            source.Source = list;
            grdClients.ItemsSource = source.View;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            txtFileName.Text = openFileDialog1.FileName;
            StreamReader SR=null;
            try
            {
                 btnFileBrowse.IsEnabled = false;
               FileInfo fi = new FileInfo(openFileDialog1.FileName);
               if (!fi.Exists)
               {
                   ShowVRAlertBox("File not exists!");
                   btnFileBrowse.IsEnabled = true;
                   return;
               }


               IQueryable<Client> cs = from cls in Client.Queryable select cls;
               foreach (Client c in cs)
               {
                   if (c != null)
                   {
                       c.Delete();
                   }
               }
               string line;
               SR = File.OpenText(fi.FullName);
               line = SR.ReadLine();
               lines = new List<string>();
               
               while (line != null)
               {
                   line = SR.ReadLine();
                   if (!string.IsNullOrEmpty(line))
                   {
                       line = line.Replace('"', ' ');
                       lines.Add(line);
                   }
               }
                currentLine = 0;
                btnFileBrowse.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new ImportClientsDelegate(ImportClient));

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if(SR!=null)
                    SR.Close();
            }
        }

        public void ImportClient()
        {
            string[] strings = lines[currentLine].Split(new[] {','}, StringSplitOptions.None);
            Client c = new Client();
            if (strings.Length > 0) c.Name = strings[0];
            if (strings.Length > 1) c.Phone = strings[1];
            if (strings.Length > 2)
            {
                string network = "Unknow";
                if (strings[2].ToLower() == "o")
                    network = "Orange";
                if (strings[2].ToLower() == "v")
                    network = "Vodafone";
                if (strings[2].ToLower() == "c")
                    network = "Cosmote";
                c.Network = network;
            }
            if (strings.Length > 3) c.Email = strings[3];
            try
            {
                c.Save();
                imported++;
            }
            catch (ActiveRecordValidationException exception)
            {
                string message = "";
                foreach (string s in exception.ValidationErrorMessages)
                {
                    message += s + Environment.NewLine;
                }
                c.Message = message;
                RoLog.Instance.WriteToLog(exception.ToString(), TracedAttribute.ERROR);
            }
            finally
            {
                list.Add(c);
                grdClients.Items.Refresh();
                source.DeferRefresh();
                currentLine++;
                txtStatus.Text = "Line "+currentLine+"/"+lines.Count;
            }
            if (currentLine>= lines.Count)
            {
                ShowVRAlertBox("Import ready" + Environment.NewLine + "Imported " + imported + " from " + lines.Count);
                return;
            }

            btnFileBrowse.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                 new ImportClientsDelegate(ImportClient));


        }




    }
}
