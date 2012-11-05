using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC
{
    public class RunningObjectsViewEngine : RazorViewEngine
    {
        private static readonly Dictionary<ISecurityPolicyContainer<Object>, KeyValuePair<Func<bool>, string>> Themes = new Dictionary<ISecurityPolicyContainer<object>, KeyValuePair<Func<bool>, string>>();

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var result = FindViewByConvention(controllerContext, viewName, masterName, useCache);
            if (result != null && result.View != null)
                return result;
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (partialViewName.Split('/').Any(item => item == "EditorTemplates" || item == "DisplayTemplates"))
            {
                var result = FindPartialViewByConvention(controllerContext, partialViewName, useCache);
                if (result != null && result.View != null)
                    return result;
            }
            var baseResult = base.FindPartialView(controllerContext, partialViewName, useCache);
            if (baseResult != null && baseResult.View != null)
                return baseResult;
            return baseResult;
        }

        private ViewEngineResult FindViewByConvention(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result;
            var actionName = controllerContext.RouteData.Values["action"].ToString();
            var themePath = GetThemePath(controllerContext);

            if (controllerContext.RouteData.Values["modelType"] != null)
            {
                var modelTypeName = controllerContext.RouteData.Values["modelType"].ToString();

                //Trying to get the partial view for a specific action and model and method
                var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
                if (action == RunningObjectsAction.Execute)
                {
                    var methodName = controllerContext.RouteData.Values["methodName"].ToString();
                    result = base.FindView(controllerContext, string.Format("{4}{0}/{1}/{2}/{3}", modelTypeName, actionName, methodName, viewName, themePath), masterName, useCache);
                    if (result.View != null)
                        return result;
                }

                //Trying to get the partial view for a specific action and model
                result = base.FindView(controllerContext, string.Format("{3}{0}/{1}/{2}", modelTypeName, actionName, viewName, themePath), masterName, useCache);
                if (result.View != null)
                    return result;

                //Trying to get the partial view for a specific modelType only
                result = base.FindView(controllerContext, string.Format("{2}{0}/{1}", modelTypeName, viewName, themePath), masterName, useCache);
                if (result.View != null)
                    return result;
            }

            //Trying to get the partial view for a specific action only
            result = base.FindView(controllerContext, string.Format("{2}{0}/{1}", actionName, viewName, themePath), masterName, useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view without action and model
            result = base.FindView(controllerContext, string.Format("{0}{1}", themePath, viewName), masterName, useCache);
            return result.View != null ? result : null;
        }

        private static string GetThemePath(ControllerContext controllerContext)
        {
            var type = ModelBinder.GetModelType(controllerContext);
            var configuration = RunningObjectsSetup.Configuration.Security.For(type);
            if (configuration != null)
            {
                var container = configuration.FindPolicyContainer(controllerContext);
                if(container != null && Themes.ContainsKey(container))
                {
                    var pair = Themes[container];
                    if(pair.Key())
                        return string.Format("{0}/", pair.Value);
                }
            }
            return string.Empty;
        }

        private ViewEngineResult FindPartialViewByConvention(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult result;
            var themePath = GetThemePath(controllerContext);
            var actionName = controllerContext.RouteData.Values["action"].ToString();
            if (controllerContext.RouteData.Values["modelType"] != null)
            {
                var modelTypeName = controllerContext.RouteData.Values["modelType"].ToString();

                //Trying to get the partial view for a specific action and model
                result = base.FindPartialView(controllerContext, string.Format("{3}{0}/{1}/{2}", modelTypeName, actionName, partialViewName, themePath), useCache);
                if (result.View != null)
                    return result;

                //Trying to get the partial view for a specific modelType only
                result = base.FindPartialView(controllerContext, string.Format("{2}{0}/{1}", modelTypeName, partialViewName, themePath), useCache);
                if (result.View != null)
                    return result;
            }

            //Trying to get the partial view for a specific action only
            result = base.FindPartialView(controllerContext, string.Format("{2}{0}/{1}", actionName, partialViewName, themePath), useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view without action and model
            result = base.FindPartialView(controllerContext, string.Format("{0}{1}", themePath, partialViewName), useCache);
            return result.View != null ? result : null;
        }

        public static void RegisterTheme<T>(ISecurityPolicyContainer<T> container, string name, Func<bool> expression) where T : class
        {
            var pair = new KeyValuePair<Func<bool>, string>(expression, name);
            if (Themes.ContainsKey(container))
                Themes[container] = pair;
            else
                Themes.Add(container, pair);
        }
    }
}