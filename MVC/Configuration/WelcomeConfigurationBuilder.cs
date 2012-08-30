using System;
using System.Web;
using System.Web.Mvc;
using RunningObjects.MVC.Html;

namespace RunningObjects.MVC.Configuration
{
    public class WelcomeConfigurationBuilder
    {
        public WelcomeConfigurationBuilder(ConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public string ViewName { get; private set; }
        public Func<object> GetModel { get; private set; }
        internal Func<RedirectResult> Redirect { get; private set; }
        public ConfigurationBuilder Configuration { get; set; }

        public WelcomeConfigurationBuilder SetModel(Func<object> expression)
        {
            GetModel = expression;
            return this;
        }

        public WelcomeConfigurationBuilder SetViewName(string viewName)
        {
            ViewName = viewName;
            return this;
        }

        public WelcomeConfigurationBuilder RedirectTo(Type modelType, RunningObjectsAction action, Func<object> expression, Func<bool> condition)
        {

            Redirect = () =>
            {
                if (condition())
                {
                    var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                    var completeUrl = urlHelper.Action(modelType, action, expression());
                    if (!string.IsNullOrEmpty(completeUrl))
                        return new RedirectResult(completeUrl);
                }
                return null;
            };
            return this;
        }
    }
}
