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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    /// <summary>
    /// Interaction logic for ApplicationGrid.xaml
    /// </summary>
    public partial class ApplicationGrid : System.Windows.Controls.Grid
    {
        public ApplicationGrid()
        {
            InitializeComponent();
            this.MouseEnter += btnCloseApp_MouseEnter;
            this.MouseLeave += btnCloseApp_MouseLeave;
        }

        public Button CloseButton
        {
            get { return this.btnCloseApp; }
        }

        private void btnCloseApp_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCloseApp.Visibility = Visibility.Visible;
        }

        private void btnCloseApp_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCloseApp.Visibility = Visibility.Collapsed;
        }
    }
}
