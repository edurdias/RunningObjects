using System;
using System.Web;
using System.Web.Routing;

namespace RunningObjects.MVC
{
    public class ActionNameConstraint : IRouteConstraint
    {
        public string[] ActionNames { get; set; }
       
        public ActionNameConstraint(params string[] actionNames)
        {
            if(actionNames == null || actionNames.Length == 0)
                throw new ArgumentNullException("actionNames");
            ActionNames = actionNames;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var actionName = values["action"].ToString();
            return Array.Exists(ActionNames, name => name.Equals(actionName));
        }
    }
}