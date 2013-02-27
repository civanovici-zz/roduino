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

namespace RoDuino.SMS.Components
{
    /// <summary>
    /// Interaction logic for RoConfirmationBox.xaml
    /// </summary>
    public partial class RoConfirmationBox 
    {
        public RoConfirmationBox(string message, FrameworkElement elementToAnimate)
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
            textBlock1.Text = message;
            this.elementToAnimate = elementToAnimate;
            FadeOut();
            this.Loaded += VRAlertBox_Loaded;
        }

        void VRAlertBox_Loaded(object sender, RoutedEventArgs e)
        {
            btnOK.Focus();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>

        public RoConfirmationBox(string message)
            : this(message, null)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        
        public new bool? DialogResult { get; set; }

        /// <summary>
        /// result of user action
        /// </summary>

        public bool MessageBoxResult
        {
            get { return this.DialogResult.HasValue && (bool)this.DialogResult; }
        }

        /// <summary>
        /// OK button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
 
        private new void OKPressed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            CloseAndFadeIn();
        }

        /// <summary>
        /// Cancel button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private new void CancelPressed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            CloseAndFadeIn();
        }

        /// <summary>
        /// close messagebox and restor 
        /// </summary>
 
        private void CloseAndFadeIn()
        {
            this.Close();
            FadeIn();
        }
    }
}