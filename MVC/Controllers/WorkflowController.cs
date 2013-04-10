using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RunningObjects.MVC.Html;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Workflow;

namespace RunningObjects.MVC.Controllers
{
    public sealed class WorkflowController : ControllerBase
    {
        public ActionResult Start(string workflowKey, Type modelType, string methodName, int index)
        {
            return Execute(workflowKey, string.Empty, modelType, methodName, index);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateRequest(true)]
        public ActionResult Start(string workflowKey, Type modelType, Method method)
        {
            return Execute(workflowKey, string.Empty, modelType, method);
        }

        public ActionResult Execute(string workflowKey, string activityKey, Type modelType, string methodName, int index)
        {
            var activity = GetActivityFor(workflowKey, activityKey);
            if (activity == null)
                throw new Exception("Workflow does not contains a start activity.");

            var activityMethod = activity.Method;
            var type = activityMethod.DeclaringType;

            var mapping = GetMappingFor(type, activityMethod, methodName, index);

            if (mapping == null)
                throw new RunningObjectsException(string.Format("No method found with name {1} for type {0}", type.PartialName(), activityMethod.Name));

            var bindingContext = MethodBinder.CreateBindingContext(ControllerContext);
            ((ValueProviderCollection)bindingContext.ValueProvider).Add(new WorkflowBindingParameterValueProvider());

            var method = Binders[typeof(Method)].BindModel(ControllerContext, bindingContext) as Method;
            if (method != null)
            {
                if (!method.Parameters.Any() || method.Descriptor.Attributes.OfType<AutoInvokeAttribute>().Any())
                {
                    return ExecuteMethodOf
                        (
                            modelType,
                            method,
                            OnSuccess(activity, null),
                            OnSuccessWithReturn(activity),
                            OnException(method),
                            HttpNotFound
                        );
                }
            }
            return !ControllerContext.IsChildAction ? (ActionResult)View("Execute", method) : PartialView("Execute", method);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateRequest(true)]
        public ActionResult Execute(string workflowKey, string activityKey, Type modelType, Method method)
        {
            var activity = GetActivityFor(workflowKey, activityKey);
            if (activity == null)
                throw new Exception("Workflow does not contains a start activity.");

            return ExecuteMethodOf
            (
                modelType,
                method,
                OnSuccess(activity, null),
                OnSuccessWithReturn(activity),
                OnException(method),
                HttpNotFound
            );
        }

        private Func<ActionResult> OnSuccess(IWorkflowActivity currentActivity, object @return)
        {
            return () =>
            {
                var nextActivity = currentActivity.NextActivity;
                if (nextActivity != null)
                {
                    if (nextActivity is IWorkflowConditionalActivity)
                    {
                        var condition = nextActivity as IWorkflowConditionalActivity;
                        nextActivity = condition.Condition(@return)
                            ? condition.IfTrueActivity
                            : condition.ElseActivity;
                    }

                    var values = new RouteValueDictionary(new
                    {
                        workflowKey = nextActivity.Workflow.Key,
                        activityKey = nextActivity.Key,
                        modelType = nextActivity.Method.DeclaringType.PartialName(),
                        methodName = nextActivity.Method.Name,
                        index = 0
                    });

                    WorkflowBindingParameterValueProvider.BindingParameters.Clear();
                    if (nextActivity.Binding != null)
                    {
                        var binding = nextActivity.Binding(@return);
                        if (binding != null)
                        {
                            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(binding))
                                WorkflowBindingParameterValueProvider.BindingParameters.Add(prop.Name, prop.GetValue(binding));
                        }
                    }

                    return RedirectToAction(RunningObjectsAction.Execute.ToString(), values);
                }
                return GetRedirectOnFinish();
            };
        }

        private Func<object, ActionResult> OnSuccessWithReturn(IWorkflowActivity nextActivity)
        {
            return @return => OnSuccess(nextActivity, @return)();
        }

        private Func<Exception, ActionResult> OnException(object model)
        {
            return ex =>
            {
                ViewBag.Exceptions = Exceptions;
                return View(model);
            };
        }

        internal static IWorkflow GetWorkflow(string key)
        {
            var workflows = RunningObjectsSetup.Configuration.Workflows;
            if (!workflows.Exists(key))
                throw new ArgumentException(string.Format("Workflow {0} does not exists.", key), "key");
            return workflows.For(key);
        }

        private static MethodMapping GetMappingFor(Type type, MethodBase method, string methodName, int index)
        {
            var typeMapping = ModelMappingManager.MappingFor(type);
            var methods = typeMapping.StaticMethods;
            return methods.FirstOrDefault(m => m.Method == method && m.MethodName == methodName && m.Index == index);
        }

        private static IWorkflowActivity GetActivityFor(string workflowKey, string activityKey)
        {
            var workflow = GetWorkflow(workflowKey);
            if (string.IsNullOrEmpty(activityKey))
                return workflow.StartActivity;
            return FindActivity(activityKey, workflow.StartActivity);
        }

        private static IWorkflowActivity FindActivity(string activityKey, IWorkflowActivity activity)
        {
            if (activity != null && activity.Key != activityKey)
            {
                if (activity is IWorkflowConditionalActivity)
                {
                    var condition = activity as IWorkflowConditionalActivity;
                    activity = FindActivity(activityKey, condition.IfTrueActivity)
                               ?? FindActivity(activityKey, condition.ElseActivity)
                               ?? FindActivity(activityKey, condition.NextActivity);
                }
                else
                    activity = FindActivity(activityKey, activity.NextActivity);
            }
            return activity;
        }

        private ActionResult GetRedirectOnFinish()
        {
            var redirectTo = ControllerContext.HttpContext.Request["redirectTo"];
            if (!string.IsNullOrEmpty(redirectTo))
                return Redirect(redirectTo);

            var defaultRedirect = RunningObjectsSetup.Configuration.DefaultRedirect;
            if (defaultRedirect != null)
            {
                var routeValues = defaultRedirect.Action != RunningObjectsAction.Welcome
                                      ? LinkExtensions.CreateRouteValueDictionary(defaultRedirect.Type, defaultRedirect.Arguments)
                                      : null;

                return RedirectToAction(defaultRedirect.Action.ToString(), "Presentation", routeValues);
            }
            return RedirectToAction(RunningObjectsAction.Welcome.ToString(), "Presentation");
        }
    }

    public class WorkflowBindingParameterValueProvider : IValueProvider
    {
        internal static IDictionary<string, object> BindingParameters
        {
            get
            {
                if (HttpContext.Current.Session["BindingParameters"] == null)
                    HttpContext.Current.Session["BindingParameters"] = new Dictionary<string, object>();
                return (IDictionary<string, object>)HttpContext.Current.Session["BindingParameters"];
            }
        }

        public bool ContainsPrefix(string prefix)
        {
            return CollectionContainsPrefix(BindingParameters.Keys, prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            var rawValue = BindingParameters.ContainsKey(key) ? BindingParameters[key] : null;
            return rawValue == null ? null : new ValueProviderResult(rawValue, Convert.ToString(rawValue), CultureInfo.CurrentCulture);
        }

        
        public static bool CollectionContainsPrefix(IEnumerable<string> collection, string prefix)
        {
            foreach (var key in collection)
            {
                if (key != null)
                {
                    if (prefix.Length == 0)
                        return true;

                    if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        if (key.Length == prefix.Length)
                            return true;

                        switch (key[prefix.Length])
                        {
                            case '.':
                            case '[':
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}