using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using PostSharp.Laos;

namespace RoDuino.SMS.Bll.Attributes
{
    [Serializable]
    public class ARSessionByRequestAttribute : OnMethodBoundaryAspect
    {
        private DateTime now;

        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            BeginARSession();
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            CloseARSession();
        }

        #region "private"

        /// <summary>
        /// starts a new AR sessionscope
        /// </summary>
        private void BeginARSession()
        {
            try
            {
                now = DateTime.Now;
                string logMessage;
                //if there is one, flush it
                if (RoSession.Instance["nh.sessionscope"] != null)
                {
                    SessionScope scope = RoSession.Instance["nh.sessionscope"] as SessionScope;
                    if (scope != null)
                    {
                        scope.Flush();
                    }
                    logMessage = " maintained opened"; //used on redirects
                }
                //create one if there is none
                else
                {
                    RoSession.Instance["nh.sessionscope"] = new SessionScope(FlushAction.Auto);
                    logMessage = " opened";
                }
                logMessage = "nh.sessionscope " + RoSession.Instance["nh.sessionscope"].GetHashCode() + logMessage;
                RoLog.Instance.WriteToLog("");
                Console.WriteLine("");
                RoLog.Instance.WriteToLog(logMessage);
                Console.WriteLine(logMessage);
            }
            catch (ActiveRecordException ex)
            {
                RoLog.Instance.WriteToLog("Problems initializing the session:" + ex.Message + "/r/n" + ex,
                                          TracedAttribute.ERROR);
            }
        }


        /// <summary>
        /// ends the current AR sessionscope, from vr session
        /// </summary>
        private void CloseARSession()
        {
            SessionScope scope = RoSession.Instance["nh.sessionscope"] as SessionScope;
            try
            {
                if (scope != null)
                {
                    string message = "nh.sessionscope " + RoSession.Instance["nh.sessionscope"].GetHashCode() +
                                     " closing, took " + (DateTime.Now.Subtract(now).Seconds) + " seconds.";
                    RoLog.Instance.WriteToLog(message);
                    Console.WriteLine(message);
                    RoLog.Instance.WriteToLog("");
                    Console.WriteLine("");
                    scope.Flush();
                    scope.Dispose();
                    RoSession.Instance["nh.sessionscope"] = null;
                }
            }
            catch (Exception ex)
            {
                RoLog.Instance.WriteToLog("Problems with the session:" + ex.Message + "/r/n" + ex, TracedAttribute.ERROR);
            }
        }

        #endregion
    }
}
