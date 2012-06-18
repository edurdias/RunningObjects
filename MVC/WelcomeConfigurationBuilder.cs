using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunningObjects.MVC
{
    public class WelcomeConfigurationBuilder
    {
        public WelcomeConfigurationBuilder(ConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public string ViewName { get; set; }
        public Func<object> GetModel { get; private set; }
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
    }
}
