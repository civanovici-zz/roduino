using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Controllers.Base;

namespace RoDuino.SMS.Controllers
{
    public class MessagesController : BaseController
    {
        public void Index()
        {
            IEqualityComparer<SmsHistory> messageComparer=new MessageComparer<SmsHistory>();
            var list = (from h in SmsHistory.Queryable orderby h.Message ascending select h).Distinct(messageComparer);
            PropertyBag["histories"] = list.ToList();
            RenderView("Messages/Index");
        }
    }

    public class MessageComparer<T> : IEqualityComparer<SmsHistory>
    {
        public bool Equals(SmsHistory x, SmsHistory y)
        {
            return x.Message == y.Message;
        }

        public int GetHashCode(SmsHistory obj)
        {
            return obj.GetHashCode();
        }
    }
}
