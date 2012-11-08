using System;
using System.Web.Mvc;

namespace RunningObjects.MVC.Logging
{
    public abstract class LoggingProvider
    {
        public void ActionExecuting(ActionExecutingContext context)
        {
            Executing(RunningObjectsAction.Welcome.GetAction(context.ActionDescriptor.ActionName), context);
        }

        public void ActionExecuted(ActionExecutedContext context)
        {
            Executed(RunningObjectsAction.Welcome.GetAction(context.ActionDescriptor.ActionName), context);
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            while (ex.InnerException != null)
                ex = ex.InnerException;


            OnException(RunningObjectsAction.Welcome.GetAction(context.RequestContext.RouteData.Values["action"].ToString()), ex);
        }

        public abstract void Executing(RunningObjectsAction action, ActionExecutingContext context);
        protected abstract void Executed(RunningObjectsAction action, ActionExecutedContext context);
        public abstract void OnException(RunningObjectsAction action, Exception ex);
    }
}