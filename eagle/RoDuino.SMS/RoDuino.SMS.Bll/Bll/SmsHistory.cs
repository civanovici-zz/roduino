using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using RoDuino.SMS.Bll.Bll.Base;

namespace RoDuino.SMS.Bll.Bll
{
    [ActiveRecord]
    public class SmsHistory:BaseItem<SmsHistory>
    {
//        [BelongsTo("ClientId")]
//        public Client Client { get; set; }

        [Property]
        public string ClientName { get; set; }

        [Property]
        public string ClientPhone { get; set; }

        [Property]
        public string Message { get; set; }

        [Property]
        public string Date { get; set; }

        [Property]
        public string Status { get; set; }

        public bool HasError { get; set; }
    }
}
