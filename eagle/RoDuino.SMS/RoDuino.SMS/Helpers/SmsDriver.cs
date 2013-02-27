using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Util;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS.Helpers
{
    public class SmsDriver
    {
        private static SmsDriver instance;
        private SerialPort myport;
        ModemSettings settings;

        private SmsDriver()
        {
        }

        public static SmsDriver Instance
        {
            get
            {
                if(instance==null)
                    instance=new SmsDriver();
                return instance;
            }
        }

        public string SendSms(string phoneNumber, string message)
        {
//            Thread.Sleep(100);
//            return true;
            string sleepTimeS = ConfigurationSettings.AppSettings["sleep_time"];
            int sleepTime=2000;
            int.TryParse(sleepTimeS, out sleepTime);
            if (sleepTime == 0) sleepTime = 2000;
            string response="";
            try
            {
                if (settings == null)
                    InitModem();

                if (!myport.IsOpen)
                {
                    myport.Open();
                    Console.WriteLine("Opening port");
                    RoLog.Instance.WriteToLog("Opening port");
                }

                myport.DiscardInBuffer();
                myport.DiscardOutBuffer();
                myport.Write("AT+cgmm\r");
                Thread.Sleep(sleepTime);
                string DeviceName = myport.ReadExisting();
                Console.WriteLine("Device:->" + DeviceName + "<--");
                RoLog.Instance.WriteToLog("Device:->" + DeviceName + "<--");
                myport.DiscardOutBuffer();
                myport.DiscardInBuffer();

                myport.Write("AT+CMGF=1\r");
                myport.Write("AT+CMGS=\"");
                myport.Write(phoneNumber);
                myport.Write("\"\r");
                myport.Write(message + "\x001a");
                Thread.Sleep(sleepTime);
                string messageStatus = myport.ReadExisting();
                if (messageStatus.Contains("OK"))
                    response = "";
                Console.WriteLine("message sent?->" + messageStatus + "<-- to "+phoneNumber);
                RoLog.Instance.WriteToLog("message sent?->" + messageStatus + "<-- to " + phoneNumber);
                myport.DiscardOutBuffer();
                myport.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                RoLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
                response = res.ResourceManager.GetString("MessageError");
            }
            finally
            {
                if (myport.IsOpen)
                {
                    myport.Close();
                }
            }
            return response;
        }

        public void InitModem()
        {
            //read settings
            settings = ModemSettings.FindFirst();
            if(myport==null)myport=new SerialPort();
            if(myport.IsOpen)
                myport.Close();
            myport.PortName = settings.Port;
            myport.BaudRate = settings.BitPerSec;// 115200;
            myport.Parity = settings.Parity;// Parity.None;
            myport.StopBits = settings.StopBits;// StopBits.One;
            myport.DataBits =settings.DataBits;
            myport.ReadBufferSize = 10000;
            myport.ReadTimeout = 1000;
            myport.WriteBufferSize = 10000;
            myport.WriteTimeout = 10000;
            myport.RtsEnable = true;
        }
    }
}
