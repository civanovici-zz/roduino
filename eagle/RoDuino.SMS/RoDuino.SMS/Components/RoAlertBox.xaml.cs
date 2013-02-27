using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RoDuino.SMS.Components
{
    /// <summary>
    /// Interaction logic for RoAlertBox.xaml
    /// </summary>
    public partial class RoAlertBox 
    {
        public RoAlertBox(string message, FrameworkElement elementToAnimate)
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
            txtAlert.Text = message;
            this.elementToAnimate = elementToAnimate;
            FadeOut();
            this.Loaded += VRAlertBox_Loaded;
        }
        public RoAlertBox(string message)
            : this(message, null)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void VRAlertBox_Loaded(object sender, RoutedEventArgs e)
        {
            btnOk.Focus();
        }

        [TypeConverter(typeof(DialogResultConverter))]
        public new bool? DialogResult { get; set; }
    }
}
