using System;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace RunningObjects.Core.Logging
{
    public class DefaultLoggingProvider : LoggingProvider
    {
        public DefaultLoggingProvider()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public Logger Logger { get; private set; }


        public override void Executing(RunningObjectsAction action, ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            Logger.Info("{0}:Executing:{1}:{2}", action, request.HttpMethod, request.RawUrl);
        }

        protected override void Executed(RunningObjectsAction action, ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var status = context.Result is HttpStatusCodeResult
                             ? ((HttpStatusCodeResult) context.Result).StatusCode
                             : 200;


            Logger.Info("{0}:Executed:{1}:{2}:{3}", action, request.HttpMethod, request.RawUrl, status);
        }

        public override void OnException(RunningObjectsAction action, Exception ex)
        {
            var request = HttpContext.Current.Request;
            Logger.Error("{0}:Error:{3}:{4}: {1} \n {2}", action, ex.Message, ex.StackTrace, request.HttpMethod, request.RawUrl);
        }
    }
}