using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RunningObjects.Core.Mapping;

namespace RunningObjects.Core.Security
{
    internal class SecurityHandlerAttribute : ActionFilterAttribute
    {
        public SecurityHandlerAttribute(SecurityConfigurationBuilder builder)
        {
            Builder = builder;
        }

        protected SecurityConfigurationBuilder Builder { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplyConfigurationFor(filterContext, ModelBinder.GetModelType(filterContext.Controller.ControllerContext));
            base.OnActionExecuting(filterContext);
        }

        private void ApplyConfigurationFor(ActionExecutingContext filterContext, Type type)
        {
            var configuration = Builder.For(type);
            if (configuration != null)
            {
                var container = configuration.FindPolicyContainer(filterContext.Controller.ControllerContext);
                if (container != null)
                    ApplyPolicies(filterContext, container);
                else if(type != typeof(object))
                    ApplyConfigurationFor(filterContext, typeof (object));
            }
        }

        private void ApplyPolicies(ActionExecutingContext filterContext, ISecurityPolicyContainer<object> container)
        {
            var context = new SecurityPolicyContext
            {
                ControllerContext = filterContext.Controller.ControllerContext
            };

            if (Builder.IsAuthenticationConfigured)
            {
                var authentication = Builder.Authentication<Object>();
                context.IsAuthenticated = authentication.IsAuthenticated();
                context.CurrentUserRoles = authentication.GetRoles();
            }

            if (container.Policies.Any(policy => !policy.Authorize(context)))
            {
                if (Builder.IsAuthenticationConfigured)
                {
                    var authentication = Builder.Authentication<Object>();
                    if (!authentication.IsAuthenticated())
                    {
                        var mapping = ModelMappingManager.MappingFor(authentication.Type);
                        var method = mapping.StaticMethods.FirstOrDefault(m => m.Name == authentication.LoginWith().Name);
                        if (method != null)
                        {
                            var route = new
                                            {
                                                action = "Execute",
                                                controller = "Presentation",
                                                methodName = method.MethodName,
                                                index = method.Index,
                                                modelType = mapping.ModelType.PartialName(),
                                                redirectTo = filterContext.HttpContext.Request.Url.ToString()
                                            };
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(route));
                        }
                        else
                            filterContext.Result = new HttpNotFoundResult();
                    }
                    else
                        filterContext.Result = new HttpNotFoundResult();
                }
                else
                    filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}