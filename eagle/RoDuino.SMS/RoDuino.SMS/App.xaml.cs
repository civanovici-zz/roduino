using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using log4net.Config;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Exceptions;
using res = RoDuino.SMS.Properties.Resources;

namespace RoDuino.SMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string title;

        public App()
        {
            //this.StartupUri = new Uri("Splash.xaml", UriKind.Relative);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            title = res.ResourceManager.GetString("Application_Title") + " " + version;
            try
            {
                XmlConfigurator.Configure();
                ResourceDictionary rd;
                Uri uri = new Uri("RoDuino.SMS;component/Skins/ShineBlack/ShineBlackResources.xaml", UriKind.Relative);
                //                Uri uri = new Uri("VRFramework;component/Skins/ShineWhite/ShineWhiteResources.xaml", UriKind.Relative);
                rd = LoadComponent(uri) as ResourceDictionary;
                Current.Resources = rd;
            }
            catch (Exception ex)
            {
                RoLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
                Console.WriteLine(ex);
            }
            try
            {
                string currentLanguage = ConfigurationManager.AppSettings["currentLanguage"];
                SMS.Properties.Resources.Culture = new CultureInfo(currentLanguage);
            }
            catch (Exception ex)
            {
                RoLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
            }



//            LoadVRShapeSettingsFromRegistries();
//            NHibernateProfiler.Initialize();
        }


        [Traced(TracedAttribute.INFO)]
        protected override void OnNavigated(NavigationEventArgs e)
        {
            //HideNavigationControls(e);
            //base.OnNavigated(e);

            //            if (navigator.NavigationService == null)
            //            {
            //            if (e.Navigator is NavigationWindow)
            //            {
            //                NavigationService ns = ((Page)e.Content).NavigationService;
            //                Navigator navigator = (Navigator) NavigatorFactory.Navigator;
            //                navigator.NavigationService = ns;
            //            }
        }


        //[Traced(0)]
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //prevent having exceptions while logging the exception
            try
            {
                //                Exception exception = (e.Exception is NavigationException) ? e.Exception : e.Exception.InnerException;
                //                if (exception == null) exception = e.Exception;
                //                LogExceptionAndShowMessageBox(exception);


                //1.transform exception into something that can be displayed better
//                FriendlyException fex = ApplicationExceptionLogger.GetFriendlyException(e.Exception);
//
                //2.log exception
//                string formatError = String.Format(
//                    "EXCEPTION:{0},\n FriendlyMessage={1}\n  Message={2}\n StackTrace={3}\n Exception={4}",
//                    fex.ActualException != null ? fex.ActualException.GetType().ToString() : "",
//                    fex.FriendlyMessage ?? "",
//                    fex.ActualMessage ?? "",
//                    fex.ActualStackTrace ?? "",
//                    fex.ActualException);
                RoLog.Instance.WriteToLog(e.Exception.ToString(), TracedAttribute.ERROR);

//                SendToEventLog(formatError);



                MessageBoxResult result = MessageBox.Show(res.ResourceManager.GetString("Oops") + "\n\n" +
                                                          e.Exception + "\n\n" +
                                                          res.ResourceManager.GetString("ShutdownApplication"),
                                                          "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    // Return exit code
                    this.Shutdown(-1);
                }
            }
            finally
            {
                // Prevent default unhandled exception processing
                e.Handled = true;
            }
        }

        
       
    }
}
