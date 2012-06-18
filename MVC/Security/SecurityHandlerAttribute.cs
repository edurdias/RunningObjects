using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC.Security
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
            var controllerContext = filterContext.Controller.ControllerContext;

            var bindingContext = new ModelBindingContext
            {
                ModelState = controllerContext.Controller.ViewData.ModelState,
                ModelMetadata = controllerContext.Controller.ViewData.ModelMetadata,
                ModelName = TypeBinder.ModeTypeKey,
                ValueProvider = controllerContext.Controller.ValueProvider
            };

            var type = (Type)ModelBinders.Binders[typeof(Type)].BindModel(controllerContext, bindingContext);

            var configuration = Builder.For(type);
            if (configuration != null)
            {
                var container = configuration.FindPolicyContainer(controllerContext);
                if (container != null)
                {
                    var context = new SecurityPolicyContext
                    {
                        ControllerContext = controllerContext
                    };

                    if (Builder.IsAuthenticationConfigured)
                    {
                        var authentication = Builder.Authentication<Object>();
                        context.AuthenticationStatus = authentication.GetStatusFrom();
                        context.CurrentUserRoles = authentication.GetRolesFrom();
                    }

                    if (container.Policies.Any(policy => !policy.Authorize(context)))
                    {
                        if (Builder.IsAuthenticationConfigured)
                        {
                            var authentication = Builder.Authentication<Object>();
                            if (!authentication.GetStatusFrom()())
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

            base.OnActionExecuting(filterContext);
        }
    }
}