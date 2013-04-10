using System.Web.Mvc;

namespace RunningObjects.Core
{
    public static class ControllerExtensions
    {
        internal static ActionDescriptor GetActionDescriptor(this ControllerContext controllerContext, RunningObjectsAction action)
        {
            return GetActionDescriptor(controllerContext, action.ToString());
        }

        internal static ActionDescriptor GetActionDescriptor(this ControllerContext controllerContext, string actionName)
        {
            var controllerDescriptor = new ReflectedControllerDescriptor(controllerContext.Controller.GetType());
            return controllerDescriptor.FindAction(controllerContext, actionName);
        }
    }
}