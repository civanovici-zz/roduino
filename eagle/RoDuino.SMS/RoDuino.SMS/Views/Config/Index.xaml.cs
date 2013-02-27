using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;

namespace RoDuino.SMS.Views.Config
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index 
    {
        private IList<ListItem> ports=new List<ListItem>();
        private IList<ListItem> bitsPerSec = new List<ListItem>();
        private IList<ListItem> dataBits = new List<ListItem>();
        private IList<ListItem> parity = new List<ListItem>();
        private IList<ListItem> stopBits = new List<ListItem>();
        private IList<ListItem> flowControl = new List<ListItem>();
        ModemSettings settings;

        public Index()
        {
            InitializeComponent();
            for(int i=0;i<255;i++)
            {
//                ports.Add(new {Name = "COM" + (i + 1)});
                ports.Add(new ListItem() { Id = i, Name = "COM" + (i + 1) });

            }
            bitsPerSec.Add(new ListItem() { Name = "75",Value = 75});
            bitsPerSec.Add(new ListItem() { Name = "110",Value = 110});
            bitsPerSec.Add(new ListItem() { Name = "134",Value = 134});
            bitsPerSec.Add(new ListItem() { Name = "150" ,Value = 150});
            bitsPerSec.Add(new ListItem() { Name = "300",Value = 300});
            bitsPerSec.Add(new ListItem() { Name = "600" ,Value = 600});
            bitsPerSec.Add(new ListItem() { Name = "1200" ,Value = 1200});
            bitsPerSec.Add(new ListItem() { Name = "1800",Value = 1800});
            bitsPerSec.Add(new ListItem() { Name = "2400",Value = 2400 });
            bitsPerSec.Add(new ListItem() { Name = "4800" ,Value = 4800});
            bitsPerSec.Add(new ListItem() { Name = "7200" ,Value = 7200});
            bitsPerSec.Add(new ListItem() { Name = "9600",Value = 9600 });
            bitsPerSec.Add(new ListItem() { Name = "14400" ,Value = 14400});
            bitsPerSec.Add(new ListItem() { Name = "19200" ,Value = 19200});
            bitsPerSec.Add(new ListItem() { Name = "38400" ,Value = 38400});
            bitsPerSec.Add(new ListItem() { Name = "57600",Value = 57600 });
            bitsPerSec.Add(new ListItem() { Name = "115200" ,Value = 115200});
            bitsPerSec.Add(new ListItem() { Name = "128000",Value = 128000 });
            dataBits.Add(new ListItem() { Name = "4",Value = 4});
            dataBits.Add(new ListItem() { Name = "5" ,Value = 5});
            dataBits.Add(new ListItem() { Name = "6" ,Value = 6});
            dataBits.Add(new ListItem() { Name = "7",Value = 7 });
            dataBits.Add(new ListItem() { Name = "8" ,Value = 8});
            parity.Add(new ListItem() { Name = Parity.Even.ToString(),Value  = Parity.Even});
            parity.Add(new ListItem() { Name = "Odd" ,Value = Parity.Odd});
            parity.Add(new ListItem() { Name = "None" ,Value = Parity.None});
            parity.Add(new ListItem() { Name = "Mark" ,Value = Parity.Mark});
            parity.Add(new ListItem() { Name = "Space" ,Value = Parity.Space});
            stopBits.Add(new ListItem() { Name = "1",Value = StopBits.One });
            stopBits.Add(new ListItem() { Name = "1.5" ,Value = StopBits.OnePointFive});
            stopBits.Add(new ListItem() { Name = "2" ,Value = StopBits.Two});
            flowControl.Add(new ListItem() { Name = "Xon/Xoff" });
            flowControl.Add(new ListItem() { Name = "Hardware" });
            flowControl.Add(new ListItem() { Name = "None" });

        }

        public override void DataBind()
        {
            settings = (ModemSettings) PropertyBag["settings"];
            this.DataContext = settings;

            DataBindModel();
        }

       

        public override void Navigated()
        {
            btnSave.Click += btnSave_Click;
            cbxPorts.SelectionChanged += cbxPorts_SelectionChanged;
            cbxBitsPerSec.SelectionChanged += cbxBitsPerSec_SelectionChanged;
            cbxDataBits.SelectionChanged += cbxDataBit_SelectionChanged;
            cbxFlowControl.SelectionChanged += cbxFlowControl_SelectionChanged;
            cbxParity.SelectionChanged += cbxParity_SelectionChanged;
            cbxStopBits.SelectionChanged += cbxStopBits_SelectionChanged;
        }



        public override void Navigating()
        {
            btnSave.Click -= btnSave_Click;
            cbxPorts.SelectionChanged -= cbxPorts_SelectionChanged;
            cbxBitsPerSec.SelectionChanged -= cbxBitsPerSec_SelectionChanged;
            cbxDataBits.SelectionChanged -= cbxDataBit_SelectionChanged;
            cbxFlowControl.SelectionChanged -= cbxFlowControl_SelectionChanged;
            cbxParity.SelectionChanged -= cbxParity_SelectionChanged;
            cbxStopBits.SelectionChanged -= cbxStopBits_SelectionChanged;
        }

        void cbxParity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.Parity= (Parity) ((ListItem) ((ComboBox) sender).SelectedItem).Value;
        }
        void cbxStopBits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.StopBits= (StopBits) ((ListItem) ((ComboBox) sender).SelectedItem).Value;
        }
        void cbxFlowControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.FlowControl= ((ListItem) ((ComboBox) sender).SelectedItem).Name;
        }
        void cbxDataBit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.DataBits= (int) ((ListItem) ((ComboBox) sender).SelectedItem).Value;
        }
        void cbxBitsPerSec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.BitPerSec = (int) ((ListItem) ((ComboBox) sender).SelectedItem).Value;
        }
        void cbxPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.Port = ((ListItem) ((ComboBox) sender).SelectedItem).Name;
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            PropertyBag["settings"] = settings;
            IDictionary prop = NavigatorFactory.Navigator.Get("Config", "Save", PropertyBag);
            if (prop.Contains("flash"))
            {
                Flash flash = (Flash)prop["flash"];
                ShowVRAlertBox(flash.Error);
            }
            settings = (ModemSettings)prop["settings"];
            DataBindModel();
        }

        private void DataBindModel()
        {
            cbxPorts.ItemsSource = ports;
            cbxPorts.DisplayMemberPath = "Name";
//            cbxPorts.SelectedValue = settings.Port;
            cbxPorts.SelectedItem = (from p in ports where p.Name == settings.Port select p).FirstOrDefault();

            cbxBitsPerSec.ItemsSource = bitsPerSec;
            cbxBitsPerSec.DisplayMemberPath = "Name";
            cbxBitsPerSec.SelectedItem = (from p in bitsPerSec where ((int)p.Value) == settings.BitPerSec select p).FirstOrDefault();

            cbxDataBits.ItemsSource = dataBits;
            cbxDataBits.DisplayMemberPath = "Name";
            cbxDataBits.SelectedItem = (from p in dataBits where ((int)p.Value)== settings.DataBits select p).FirstOrDefault();


            cbxParity.ItemsSource = parity;
            cbxParity.DisplayMemberPath = "Name";
            cbxParity.SelectedItem = (from p in parity where ((Parity)p.Value)== settings.Parity select p).FirstOrDefault(); ;

            cbxStopBits.ItemsSource = stopBits;
            cbxStopBits.DisplayMemberPath = "Name";
            cbxStopBits.SelectedItem = (from p in stopBits where ((StopBits)p.Value)== settings.StopBits select p).FirstOrDefault();

            cbxFlowControl.ItemsSource = flowControl;
            cbxFlowControl.DisplayMemberPath = "Name";
            cbxFlowControl.SelectedItem = (from p in flowControl where p.Name == settings.FlowControl select p).FirstOrDefault(); ;
        }
    }
}
