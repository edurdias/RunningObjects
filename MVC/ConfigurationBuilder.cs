using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC
{
    public class ConfigurationBuilder
    {
        public ConfigurationBuilder()
        {
            Welcome = new WelcomeConfigurationBuilder(this);
            Security = new SecurityConfigurationBuilder(this);
        }

        public WelcomeConfigurationBuilder Welcome { get; private set; }
        public SecurityConfigurationBuilder Security { get; private set; }
    }
}
