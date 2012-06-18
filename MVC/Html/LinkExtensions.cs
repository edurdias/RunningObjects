using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace RunningObjects.MVC.Html
{
    public static class LinkExtensions
    {
        public static string Action(this UrlHelper urlHelper, Type modelType, RunningObjectsAction action, object arguments)
        {
            //TODO:Create constant for string value
            return urlHelper.Action(action.ToString(), "Presentation", CreateRouteValueDictionary(modelType, arguments));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, Type modelType, RunningObjectsAction action, object arguments = null)
        {
            return ActionLink(htmlHelper, linkText, modelType, action, arguments, null);
        }

        private static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, Type modelType, RunningObjectsAction action, object arguments, object htmlAttributes)
        {
            var routeValues = new RouteValueDictionary(htmlHelper.ViewContext.RouteData.Values);
            try
            {
                htmlHelper.ViewContext.RouteData.Values.Clear();
                return htmlHelper.ActionLink(linkText, action.ToString(), "Presentation",
                                             CreateRouteValueDictionary(modelType, arguments),
                                             HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }finally
            {
                foreach (var routeValue in routeValues)
                    htmlHelper.ViewContext.RouteData.Values.Add(routeValue.Key, routeValue.Value);
            }
        }

        private static RouteValueDictionary CreateRouteValueDictionary(Type modelType, object arguments)
        {
            return new RouteValueDictionary(arguments) { { "modelType", modelType.PartialName() } };
        }
    }
}
