using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Tests.Base;

namespace RoDuino.SMS.Tests.Unit
{
    [TestFixture]
    public class GenerateDefaultData : BaseFactoryTest
    {
        [Test]
        public void GenerateEmptyDatabase()
        {
            User u = new User() { Username = "admin", Password = "admin" };
            u.Save();
        }

        [Test]
        public void GenerateData()
        {
            User u=new User(){Username = "admin",Password = "admin"};
            u.Save();

            //generate 100 users
            for(int i=0;i<100;i++)
            {
                Client c=new Client(){Name = "client "+i,Network = "Orange",Phone = "0745961116"};
                c.Save();
            }

            ModemSettings modemSettings = new ModemSettings()
                                              {
                                                  Port = "COM1",
                                                  BitPerSec = 115200,
                                                  DataBits = 8,
                                                  Parity =Parity.None,
                                                  StopBits =StopBits.One,
                                                  FlowControl = "NONE"
                                              };
            modemSettings.Save();

            var clients = Client.FindAll();
            
            for(int i=0;i<5;i++)
            {
                for(int j=0;j<6;j++)
                {
                    var date = DateTime.Now.AddDays(j);
                    SmsHistory h = new SmsHistory()
                                       {
//                                           Client = clients[i],
                                           Date = date.ToString(),
                                           Message = "Mesajul Nr " + j,
                                           Status = "Send ok",
                                           ClientName = clients[i].Name,
                                           ClientPhone = clients[i].Phone
                                       };
                    h.Save();
                }
            }
        }
    }
}
