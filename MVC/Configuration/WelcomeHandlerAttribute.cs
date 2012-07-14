using System;
using System.Web.Mvc;

namespace RunningObjects.MVC.Configuration
{
    public class WelcomeHandlerAttribute : ActionFilterAttribute
    {
        public WelcomeHandlerAttribute(WelcomeConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public WelcomeConfigurationBuilder Configuration { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.RouteData.Values["action"].ToString();
            var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
            if (action == RunningObjectsAction.Welcome)
            {

                //dynamic viewData = filterContext.Controller.ViewData;
                //viewData.Model = Configuration.GetModel();
                //filterContext.Result = new ViewResult
                //{
                //    ViewName = Configuration.ViewName,
                //    ViewData = viewData,
                //    TempData = filterContext.Controller.TempData,
                //    ViewEngineCollection = ViewEngines.Engines

                //};
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
