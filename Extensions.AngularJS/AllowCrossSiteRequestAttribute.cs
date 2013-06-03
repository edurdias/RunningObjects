using System.Web.Mvc;

namespace RunningObjects.Extensions.AngularJS
{
	public class AllowCrossSiteRequestAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
			base.OnActionExecuting(filterContext);
		}
	}
}