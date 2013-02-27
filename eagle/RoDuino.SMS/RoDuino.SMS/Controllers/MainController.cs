using System;
using System.Collections.Generic;
using System.Linq;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Notifications;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Views.Main;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Controllers
{
    public class MainController:BaseController
    {
        public void List()
        {
            IQueryable<Client> clients = from cls in RoDuino.SMS.Bll.Bll.Client.Queryable where (cls.IsDeleted == false) select cls;
            PropertyBag["clients"] = clients;
            RenderView("Main/List");
        }

        public void Add()
        {
            Client client=new Client();
            PropertyBag["client"] = client;
            RenderView("Main/Edit");
        }

        public void Edit(Client client)
        {
            PropertyBag["client"] = client;
            RenderView("Main/Edit");
        }

        public void Save(Client client)
        {
            try
            {
                client.Save();
//            Redirect("Main","List");
                Tabs.Instance.Close(Tabs.Instance.Current);
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.Message,TracedAttribute.ERROR);
                Flash flash =new Flash(e.Message);
                PropertyBag["client"] = client;
                PropertyBag["flash"] = flash;
                RenderView("Main/Edit");
            }
        }

        public void Delete(int[] clients)
        {
            foreach (int id in clients)
            {
                Client c = Client.Find(id);
                if(c!=null)
                {
                    c.IsDeleted = true;
                    c.Save();
                }
            }
            IQueryable<Client> cs= from cls in RoDuino.SMS.Bll.Bll.Client.Queryable where (cls.IsDeleted == false) select cls;
            PropertyBag["clients"] = cs;
        }


        public void SendSMS(int[] clients, string message)
        {
            SmsHistory history;
            IList<SmsHistory> list = new List<SmsHistory>();
            foreach (var id in clients)
            {
                Client client = Client.Find(id);
                if (client != null)
                {
                    history = new SmsHistory()
                    {
//                        Client = client,
                        ClientName = client.Name,
                        ClientPhone = client.Phone,
                        Date = DateTime.Now.ToString(),
                        Status = res.ResourceManager.GetString("MessagePending"),
                        Message = message
                    };
                    history.Save();
                    list.Add(history);
                }
            }
            PropertyBag["histories"] = list;
            RenderView("Main/SendSms");

            
        }

        public void TrySendSMS(SmsHistory history)
        {
            try
            {
                this.RaiseNotification(res.ResourceManager.GetString("BeginSendingMessageTo")+history.ClientPhone+"("+history.ClientName+")");
                string resp=SmsDriver.Instance.SendSms(history.ClientPhone, history.Message);
                if(string.IsNullOrEmpty(resp))
                {
                    history.Status = res.ResourceManager.GetString("MessageSend");
                    history.Save();
                    this.RaiseNotification(res.ResourceManager.GetString("MessageSend") + history.ClientPhone + "(" + history.ClientName + ")");
                }
                else
                {
                    history.Status = res.ResourceManager.GetString("MessageError");
                    history.Save();
                    this.RaiseNotification(res.ResourceManager.GetString("MessageError") + history.ClientPhone + "(" + history.ClientName + ")");
                    history.HasError = true;
                }

            }
            catch (Exception ex)
            {

                this.RaiseNotification(res.ResourceManager.GetString("ErrorSendingMessage") );
                RoLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
            }
            finally
            {
                PropertyBag["history"] = history;
            }
        }
    }
}
