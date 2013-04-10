using System;
using System.Collections.Generic;
using RunningObjects.Core.Configuration;

namespace RunningObjects.Core.Security
{
    public class SecurityConfigurationBuilder : IDisposable
    {
        private readonly Dictionary<Type, ITypeSecurityConfiguration<Object>> configurations = new Dictionary<Type, ITypeSecurityConfiguration<Object>>();
        private IAuthentication<Object> authentication;

        public SecurityConfigurationBuilder(ConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public ConfigurationBuilder Configuration { get; private set; }

        public ITypeSecurityConfiguration<T> For<T>() where T : class
        {
            var key = typeof(T);
            if (!Exists(key))
            {
                configurations.Add(key, new TypeSecurityConfiguration<T>());
            }
            return GetConfiguration<T>(key);
        }

        public void For<T>(ITypeSecurityConfigurationBuilder<T> builder) where T : class
        {
            builder.Configure(For<T>());
        }

        public ITypeSecurityConfiguration<Object> For(Type type)
        {
            return GetConfiguration<object>(type);
        }

        public ITypeSecurityConfiguration<Object> ForEverything()
        {
            return For<object>();
        }

        public ISecurityPolicyContainer<object> ForWelcome()
        {
            var welcome = ForEverything();
            return welcome.OnWelcome();
        }

        public bool Exists(Type type)
        {
            return configurations.ContainsKey(type);
        }

        private ITypeSecurityConfiguration<T> GetConfiguration<T>(Type type)
        {
            do
            {
                type = type ?? typeof(object);
                if (Exists(type))
                {
                    var configuration = configurations[type];
                    if (configuration != null)
                        return (ITypeSecurityConfiguration<T>)configuration;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        public bool IsAuthenticationConfigured
        {
            get { return authentication != null; }
        }

        public IAuthentication<T> Authentication<T>() where T : class
        {
            if (authentication == null)
                authentication = new Authentication<T>(this);
            return (IAuthentication<T>)authentication;
        }

        public void Dispose()
        {
        }
    }
}