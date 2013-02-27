using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace RoDuino.SMS.Components
{
    public class RoTextBox : TextBox
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.TextChanged += RoTextBox_TextChanged;
        }

        void RoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GetBindingExpression(TextBox.TextProperty) != null)
                GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
