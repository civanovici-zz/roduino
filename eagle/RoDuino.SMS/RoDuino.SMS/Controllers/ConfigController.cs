using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Notifications;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Controllers
{
    public class ConfigController : BaseController
    {
        public void Index()
        {
            ModemSettings modemSettings = ModemSettings.FindFirst();
            PropertyBag["settings"] = modemSettings;
            RenderView("Config/Index");
        }

        public void Save(ModemSettings settings)
        {
            try
            {
                settings.Save();
                PropertyBag["settings"] = settings;
                this.RaiseNotification(res.ResourceManager.GetString("SavingSettings"));
            }
            catch (Exception ex)
            {
                Flash flash = new Flash(ex.Message);
                PropertyBag["settings"] = settings;
                PropertyBag["flash"] = flash;
            }
        }
    }
}
