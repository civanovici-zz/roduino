using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using log4net;
using log4net.Config;
using NUnit.Framework;
using RoDuino.SMS.Bll;
using RoDuino.SMS.Bll.Bll;

namespace RoDuino.SMS.Tests.Base
{
    public class BaseFactoryTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BaseFactoryTest));
        private static bool initialized;
        protected string DEFAULT_LANG = "en-Us";

        [TestFixtureSetUp]
        public virtual void OneTimeTestInitialize()
        {
            if (!initialized)
            {
                XmlConfigurator.Configure();
                IConfigurationSource source = ConfigurationSettings.GetConfig("activerecord") as IConfigurationSource;

                ActiveRecordStarter.Initialize(Assembly.GetAssembly(typeof(User)), source);

                ActiveRecordStarter.GenerateCreationScripts("script.sql");
                initialized = true;

            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            ActiveRecordStarter.CreateSchema();
            return;
            //executing delete statement, take same time like create tables...

        }


        #region Mother creations ...


        protected User CreateUser(string name)
        {
            User user = new User() { Username = name, Password = name };
            user.Save();
            return user;
        }

       

        //        protected Contact CreateContact(string name, User user, Zone zone)
        //        {
        //            Contact contact=new Contact(){FirstName = name,LastName = name};
        //            contact.Owner = user;
        //            user.Contacts.Add(contact);
        //
        //            contact.Save();
        //            return contact;
        //        }
        #endregion



    }
}
