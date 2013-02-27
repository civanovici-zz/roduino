using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Controllers.Base;

namespace RoDuino.SMS.Controllers
{
    public class HistoryController : BaseController
    {
        public void Index()
        {
            var list = from h in SmsHistory.Queryable orderby h.Date descending select h;
            PropertyBag["histories"] = list.ToList();
            RenderView("History/Index");
        }
    }
}
