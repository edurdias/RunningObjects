using System;
using System.Linq;
using System.Web.Mvc;

namespace RunningObjects.MVC
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ValidateRequestAttribute : FilterAttribute, IAuthorizationFilter
    {
        public ValidateRequestAttribute(bool validate)
        {
            Validate = validate;
        }

        public ValidateRequestAttribute(bool validate, RunningObjectsAction action)
            : this(validate)
        {
            Action = action;
        }

        protected bool Validate { get; set; }

        protected RunningObjectsAction Action { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            var controllerContext = filterContext.Controller.ControllerContext;
            var actionName = (string)controllerContext.RouteData.Values["action"];
            var action = RunningObjectsAction.Welcome.GetAction(actionName);
            var bindingContext = MethodBinder.CreateBindingContext(controllerContext);

            if (action == RunningObjectsAction.Create || action == RunningObjectsAction.Execute)
            {
                var methodMapping = MethodBinder.GetMethodMapping(controllerContext, bindingContext, actionName);
                if (methodMapping != null)
                {
                    var validateInput = methodMapping.Method.GetCustomAttributes(true).OfType<ValidateRequestAttribute>().FirstOrDefault();
                    if (validateInput != null)
                        filterContext.Controller.ValidateRequest = validateInput.Validate;
                }
            }
            else
            {
                var modelType = ModelBinder.GetModelType(controllerContext);
                if (modelType != null)
                {
                    var validateInput = modelType.GetCustomAttributes(true).OfType<ValidateRequestAttribute>().FirstOrDefault();
                    if (validateInput != null && action == validateInput.Action)
                        filterContext.Controller.ValidateRequest = validateInput.Validate;
                }
            }
        }
    }
}