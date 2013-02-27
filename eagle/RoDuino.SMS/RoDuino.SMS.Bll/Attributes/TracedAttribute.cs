using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Laos;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Bll.Attributes
{
    [Serializable]
    public class TracedAttribute : OnMethodBoundaryAspect
    {
        public const int DEBUG = 2;
        public const int ERROR = 1;
        public const int FATAL = 0;
        public const int INFO = 3;
        private int debugLevel;

        private DateTime now;
        private string[] prefixes = { "", "--", "----", "------" };
        private bool debugOnConsole = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAttribute"/> class.
        /// </summary>
        /// <param name="debugLevel">The debug level.</param>
        public TracedAttribute(int debugLevel)
        {
            this.debugLevel = debugLevel;
            this.OnExceptionMessage = String.Empty;
        }

        public TracedAttribute(int debugLevel, bool debugOnConsole)
        {
            this.debugLevel = debugLevel;
            this.debugOnConsole = debugOnConsole;
            this.OnExceptionMessage = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAttribute"/> class.
        /// </summary>
        /// <param name="debugLevel">The debug level.</param>
        /// <param name="onException"></param>
        public TracedAttribute(int debugLevel, string onException)
        {
            this.debugLevel = debugLevel;
            this.OnExceptionMessage = onException;
        }

        /// <summary>
        /// Raises the <see cref=E:Entry/> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="PostSharp.Laos.MethodExecutionEventArgs"/> instance containing the event data.</param>
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            now = DateTime.Now;

            //            Console.WriteLine(String.Format("\t\t\tEntered in method {2}/{0} at {1} ", eventArgs.Method.Name, now, eventArgs.Instance != null ? eventArgs.Instance.GetType().Name : ""));

            RoLog.Instance.LastMessage = OnExceptionMessage;
            if (RoConfig.Instance.DebugLevel >= debugLevel || debugOnConsole)
            {
                string logMessage = prefixes[debugLevel];

                logMessage += String.Format("<{0}/{1}/",
                                            eventArgs.Instance != null ? eventArgs.Instance.GetType().Name : "Static",
                                            eventArgs.Method);
                if (eventArgs.GetArguments() != null)
                {
                    foreach (object o in eventArgs.GetArguments())
                    {
                        logMessage += (o == null ? "NULL|" : o + "|");
                    }
                }
                else
                {
                    logMessage += "none";
                }
                logMessage += ">";

                RoLog.Instance.Add(logMessage);
                if (debugOnConsole) Console.WriteLine(logMessage);
                RoLog.Instance.WriteToLog(logMessage, debugLevel);
                //                VRLog.Instance.WriteToLog(logMessage);
            }
        }

        /// <summary>
        /// Raises the <see cref=E:Exit/> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="PostSharp.Laos.MethodExecutionEventArgs"/> instance containing the event data.</param>
        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            if (RoConfig.Instance.DebugLevel >= debugLevel || debugOnConsole)
            {
                string returnValue = "" + (eventArgs.Method.ToString().ToLower().Contains("void")
                                               ? ""
                                               : (eventArgs.ReturnValue ?? "NULL"));
                string logMessage =
                    String.Format(prefixes[debugLevel] + "</{0}/{1}/{2}/{3}>",
                                  eventArgs.Instance != null ? eventArgs.Instance.GetType().Name : "Static",
                                  eventArgs.Method,
                                  returnValue, DateTime.Now.Subtract(now).Duration());
                RoLog.Instance.Add(logMessage);
                if (debugOnConsole) Console.WriteLine(logMessage);
                //                VRLog.Instance.WriteToLog(logMessage);
                RoLog.Instance.WriteToLog(logMessage, debugLevel);
            }

            //            Console.WriteLine(String.Format("\t\t\tExit method {2}/{0}, took {1} ms", eventArgs.Method.Name, DateTime.Now.Subtract(now).TotalMilliseconds, eventArgs.Instance != null ? eventArgs.Instance.GetType().Name : ""));
        }


        public string OnExceptionMessage { set; get; }
    }
}
