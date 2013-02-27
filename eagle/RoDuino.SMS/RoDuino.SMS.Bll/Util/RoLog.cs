using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace RoDuino.SMS.Bll.Util
{
    public class RoLog
    {

        private static RoLog instance;
        private readonly ILog logger = LogManager.GetLogger("RoLogging");
        private List<string> logs;

        private RoLog()
        {
            if (logs == null)
                logs = new List<string>();
        }

        public static RoLog Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoLog();
                }
                return instance;
            }
        }

        public List<string> Logs
        {
            get { return logs; }
        }

        /// <summary>
        /// used by Trace to set a friendly message that can be displayed to the user in case of failure/exception
        /// </summary>
        public string LastMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Adds the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public void Add(string logEntry)
        {
            this.logs.Add(logEntry);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            logs.Clear();
        }

       

        /// <summary>
        /// Writes the message to log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteToLog(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Writes the message to log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteToLog(string message, int debugLevel)
        {
            //FATAL = 0;
            //ERROR = 1;
            //DEBUG = 2;
            //INFO = 3;
            switch (debugLevel)
            {
                case 0:
                    if(logger.IsFatalEnabled) logger.Fatal(message);
                    break;
                case 1:
                    if(logger.IsErrorEnabled) logger.Error(message);
                    break;
                case 2:
                    if(logger.IsDebugEnabled) logger.Debug(message);
                    break;
                case 3:
                    if(logger.IsInfoEnabled) logger.Info(message);
                    break;
            }
            //            logger.Debug(message);
        }

    }
}
