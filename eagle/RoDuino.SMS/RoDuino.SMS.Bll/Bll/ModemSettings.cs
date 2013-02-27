using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using RoDuino.SMS.Bll.Bll.Base;

namespace RoDuino.SMS.Bll.Bll
{
    [ActiveRecord("ModemSettings")]
    public class ModemSettings:BaseItem<ModemSettings>
    {
        public ModemSettings()
        {
            this.Name = "modem";
        }

        [Property]
        public string Port { get; set; }

        [Property]
        public int BitPerSec { get; set; }

        [Property]
        public int DataBits { get; set; }

        [Property]
        public Parity Parity { get; set; }

        [Property]
        public StopBits StopBits { get; set; }

        [Property]
        public string FlowControl { get; set; }


    
    }

}
