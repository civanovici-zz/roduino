using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using RoDuino.SMS.Controllers.Base;

namespace RoDuino.SMS.Components
{
    public class TabButton : Button
    {
        public static DependencyProperty TabProperty = DependencyProperty.Register("Tab", typeof(Tab),
                                                                                   typeof(TabButton));

        public Tab Tab
        {
            get { return (base.GetValue(TabProperty) as Tab); }
            set { base.SetValue(TabProperty, value); }
        }
    }
}
