using RunningObjects.MVC.Mapping.Configuration;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC.Configuration
{
    public class ConfigurationBuilder
    {
        public ConfigurationBuilder()
        {
            Welcome = new WelcomeConfigurationBuilder(this);
            Security = new SecurityConfigurationBuilder(this);
            Mapping = new MappingConfiguration(this);
        }

        public WelcomeConfigurationBuilder Welcome { get; private set; }
        public SecurityConfigurationBuilder Security { get; private set; }
        public MappingConfiguration Mapping { get; private set; }
    }
}
