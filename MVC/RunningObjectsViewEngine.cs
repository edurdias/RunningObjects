using System.Linq;
using System.Web.Mvc;
using System;

namespace RunningObjects.MVC
{
    public class RunningObjectsViewEngine : RazorViewEngine
    {
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
            var result = default(ViewEngineResult);
            var actionName = controllerContext.RouteData.Values["action"].ToString();

            if (controllerContext.RouteData.Values["modelType"] != null)
            {
                var modelType = controllerContext.RouteData.Values["modelType"].ToString();

                //Trying to get the partial view for a specific action and model and method
                var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
                if (action == RunningObjectsAction.Execute)
                {
                    var methodName = controllerContext.RouteData.Values["methodName"].ToString();
                    result = base.FindView(controllerContext, string.Format("{0}/{1}/{2}/{3}", modelType, actionName, methodName, viewName), masterName, useCache);
                    if (result.View != null)
                        return result;
                }

                //Trying to get the partial view for a specific action and model
                result = base.FindView(controllerContext, string.Format("{0}/{1}/{2}", modelType, actionName, viewName), masterName, useCache);
                if (result.View != null)
                    return result;

                //Trying to get the partial view for a specific modelType only
                result = base.FindView(controllerContext, string.Format("{0}/{1}", modelType, viewName), masterName, useCache);
                if (result.View != null)
                    return result;
            }

            //Trying to get the partial view for a specific action only
            result = base.FindView(controllerContext, string.Format("{0}/{1}", actionName, viewName), masterName, useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view without action and model
            result = base.FindView(controllerContext, viewName, masterName, useCache);
            return result.View != null ? result : null;
        }

        private ViewEngineResult FindPartialViewByConvention(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var actionName = controllerContext.RouteData.Values["action"].ToString();
            var modelType = controllerContext.RouteData.Values["modelType"].ToString();

            //Trying to get the partial view for a specific action and model
            var result = base.FindPartialView(controllerContext, string.Format("{0}/{1}/{2}", modelType, actionName, partialViewName), useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view for a specific modelType only
            result = base.FindPartialView(controllerContext, string.Format("{0}/{1}", modelType, partialViewName), useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view for a specific action only
            result = base.FindPartialView(controllerContext, string.Format("{0}/{1}", actionName, partialViewName), useCache);
            if (result.View != null)
                return result;

            //Trying to get the partial view without action and model
            result = base.FindPartialView(controllerContext, partialViewName, useCache);
            return result.View != null ? result : null;
        }

        internal static void RegisterTheme<T>(Security.ISecurityPolicyContainer<T> container, string theme, Func<bool> expression)
        {
            throw new NotImplementedException();
        }
    }
}