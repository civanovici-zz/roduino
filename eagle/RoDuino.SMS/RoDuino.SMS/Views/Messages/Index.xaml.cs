using System;
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
using System.Windows.Shapes;
using RoDuino.SMS.Bll.Bll;

namespace RoDuino.SMS.Views.Messages
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index 
    {
        private IList<SmsHistory> list = new List<SmsHistory>();
        CollectionViewSource source = new CollectionViewSource();
        public Index()
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
        }

        public override void Navigating()
        {
        }
    }
}
