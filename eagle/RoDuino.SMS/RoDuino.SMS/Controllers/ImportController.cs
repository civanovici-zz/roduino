using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Controllers.Base;

namespace RoDuino.SMS.Controllers
{
    public class ImportController : BaseController
    {
        public void Index()
        {
            RenderView("Import/Index");
        }
    }
}
