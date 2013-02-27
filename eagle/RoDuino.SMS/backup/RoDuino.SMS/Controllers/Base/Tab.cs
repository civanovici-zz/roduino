using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Views;

namespace RoDuino.SMS.Controllers.Base
{
    public class Tab
    {

        private View view;

        public View View
        {
            get { return view; }
            set { view = value; }
        }
    }
}
