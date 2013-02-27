using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Views.Login;
using Rijndael = RoDuino.SMS.Helpers.Rijndael;
using res = RoDuino.SMS.Properties.Resources;


namespace RoDuino.SMS.Controllers
{
    public class LoginController:BaseController
    {
        private static bool ARIsInitialized = false;

        
        public void Login(string connection)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (!dir.Exists) return;

            this.PropertyBag["connections"] = GetConnectionsList(dir);
            this.PropertyBag["connection"] = connection;
            this.PropertyBag["version"] = ConfigurationManager.AppSettings["version"];
            this.PropertyBag["user"] = new User();
        }

        public void Browse(FileInfo filetocopy)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (filetocopy != null && filetocopy.Exists)
                try
                {
                    filetocopy.CopyTo(dir + "\\" + filetocopy.Name, true);
                    this.PropertyBag["connection"] = filetocopy.Name.Replace(".ppconf", "");
                }
                catch (Exception exception)
                {
                    RoLog.Instance.WriteToLog(
                        String.Format("EXCEPTION:{0},\n Message='{1},\n StackTrace={2}', Exception={3}",
                                      exception.GetType(), exception.Message, exception.StackTrace, exception), TracedAttribute.ERROR);
                    //                    MessageBox.Show("Source file same as destination file");
                }
            Redirect("Login", "Login");
        }

        
        public void Authentificate(User user, string connection)
        {
            Flash flash = new Flash();
            bool initialisatioARResult = false;
            if (!string.IsNullOrEmpty(connection))
            {
                FileInfo configFile = new FileInfo(Directory.GetCurrentDirectory() + "\\" + connection + ".ppconf");
                StreamReader sr = new StreamReader(configFile.FullName);
                string connString =GetHarcodedConfig();// Rijndael.Decrypt(sr.ReadLine());

                XmlConfigurationSource source = new XmlConfigurationSource(new StringReader(connString));
                RoSession.Instance["currentConfiguration"] = source;
                //try to initialize the selected connection, if failed, redirect to login page

                string databaseName = ExtractDatabaseNameFromConnectionString(connString);

                InitializeDatabase(connection, databaseName, user, flash);

            }

            //if there was no error initializing the database
            if (string.IsNullOrEmpty(flash.Error))
            {
                ticket = User.Login(user.Username, user.Password);

                if (ticket != Guid.Empty)
                {
                    PropertyBag["name"] = "Users";
                    //Redirect("Range", "List");
                    //                Redirect("Shape", "ViewShape");
                    //                Redirect("Tests", "ThrowControllerExceptionWithFriendlyMessage");
                }
                else
                {
                    flash.Error = res.ResourceManager.GetString("Login_CredentialsAreNotGood");
                    PropertyBag["flash"] = flash;
                    this.PropertyBag["connection"] = connection;
                    this.PropertyBag["connections"] =
                        GetConnectionsList(new DirectoryInfo(Directory.GetCurrentDirectory()), connection, user.Username);
                    this.PropertyBag["user"] = user;
                    //                RenderView("Login/Login");
                }
            }
            RenderView("Login/Login");
        }

        private string GetHarcodedConfig()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"+
                "<activerecord isWeb=\"false\" isDebug=\"false\">"+
                "<config>"+
                "<add key=\"dialect\" value=\"NHibernate.Dialect.SQLiteDialect\"/>"+
                "<add key=\"connection.driver_class\" value=\"NHibernate.Driver.SQLite20Driver\"/>"+
                "<add key=\"connection.connection_string\" value=\"Data Source=Roduino.db;Version=3;New=True\"/>"+
                "<add key=\"proxyfactory.factory_class\" value=\"NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle\"/>"+
                "<add key=\"show_sql\"                      value=\"false\" />"+
                "<add key=\"use_outer_join\"                value=\"true\" />"+
                "<add key=\"query.substitutions\"           value=\"true 1, false 0, yes 'Y', no 'N'\" />"+
                "</config>"+
                "</activerecord>";
        }


        private void InitializeDatabase(string connection, string databaseName, User user, Flash flash)
        {
            int step = 0;
            try
            {
                Assembly[] assemblies = new Assembly[]{ Assembly.Load("RoDuino.SMS.Bll") };
                if (RoSession.Instance["currentConfiguration"] == null)
                {

                    ActiveRecordStarter.Initialize(assemblies, ConfigurationManager.GetSection("activerecord") as IConfigurationSource);
                }
                else
                {
                    ActiveRecordStarter.Initialize(assemblies, (XmlConfigurationSource)RoSession.Instance["currentConfiguration"]);
                }
//                Migrator.Migrator migrator = new Migrations().Migrator;
//                if (migrator.LastVersion != migrator.CurrentVersion)
//                {
                    //                        new VRAlertBox(res.ResourceManager.GetString("Login_OldDatabaseContactAdministrator")).ShowDialog();
                    //                        Redirect("Login", "Login");
//                    if (migrator.CurrentVersion == 0)
//                    {
//                        flash.Error = String.Format(res.ResourceManager.GetString("Login_InvalidDatabaseConnection"), databaseName);
//                    }
//                    else
//                    {
//                        flash.Error = res.ResourceManager.GetString("Login_OldDatabaseContactAdministrator");
//                    }
//                    PropertyBag["flash"] = flash;
//                    this.PropertyBag["connection"] = connection;
//                    this.PropertyBag["connections"] = GetConnectionsList(new DirectoryInfo(Directory.GetCurrentDirectory()), connection, user.Username);
//                    this.PropertyBag["user"] = user;
//                    return;
//                }
//                step++;
//                DynamicFieldsUtil.Instance(new Migrations().Provider).InitializeARWithDynamicFields();
            }
            catch (Exception exception)
            {
                RoLog.Instance.WriteToLog(String.Format("EXCEPTION:{0},\n Message='{1},\n StackTrace={2}', Error={3}", exception.GetType(), exception.Message, exception.StackTrace, exception), TracedAttribute.ERROR);

                if (step == 0)//maybe the database was deleted, exceptions comes from new Migrations()
                {
                    //                        new VRAlertBox(res.ResourceManager.GetString("Login_CannotAccessDatabase")).ShowDialog();
                    flash.Error = res.ResourceManager.GetString("Login_CannotAccessDatabase");
                }
                else
                {
                    //                        new VRAlertBox(res.ResourceManager.GetString("Login_CannotConnect")).ShowDialog();
                    flash.Error = res.ResourceManager.GetString("Login_CannotConnect");
                }
                //                    Redirect("Login", "Login");
                PropertyBag["flash"] = flash;
                this.PropertyBag["connection"] = connection;
                this.PropertyBag["connections"] = GetConnectionsList(new DirectoryInfo(Directory.GetCurrentDirectory()), connection, user.Username);
                this.PropertyBag["user"] = user;

                return;
            }
            return;
        }

        
        public void AuthentificateAdmin(string connection)
        {
            User u = new User();
            u.Username = "admin";
            u.Password = "admin";
            Authentificate(u, connection);
        }

        private string ExtractDatabaseNameFromConnectionString(string connString)
        {
            string databaseName = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(connString));
            //parse the xml for connection string
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes[0].Value == "connection.connection_string")
                {
                    string[] connStr = node.Attributes[1].Value.Split(';');

                    if (connStr.Length == 3)
                    {
                        databaseName = connStr[1].Split('=')[1];
                    }
                    else
                    {
                        databaseName = connStr[1].Split('=')[1];
                    }
                    break;
                }
            }
            return databaseName;
        }

        /// <summary>
        /// load connections from the disk
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        
        private IList GetConnectionsList(DirectoryInfo dir)
        {
            return GetConnectionsList(dir, "", "");
        }

        /// <summary>
        /// load connections from disk, and set username to the one selected
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="selectedFileName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        
        private IList GetConnectionsList(DirectoryInfo dir, string selectedFileName, string username)
        {
            IList list = new ArrayList();
            IList<FileInfo> files = dir.GetFiles("*.ppconf");
            foreach (FileInfo file in files)
            {
                string filename = file.Name.Replace(file.Extension, "");

                var conObject = new ConnectionData
                {
                    FileName = filename
                };

                if (filename.Equals(selectedFileName))
                    conObject.Username = username;
                list.Add(conObject);
            }
            return list;
        }
    }
}