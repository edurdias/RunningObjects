using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using RunningObjects.MVC.Configuration;
using RunningObjects.MVC.Persistence;

namespace RunningObjects.MVC.Mapping.Configuration
{
    public class AssemblyMappingConfiguration
    {
        public static readonly Func<Type, bool> TypePredicate = t => t.IsPublic && !t.IsGenericType && !t.IsInterface && !t.IsSubclassOf(typeof(DbContext));

        public AssemblyMappingConfiguration(ConfigurationBuilder configuration, Assembly assembly, string rootNamespace)
        {
            Configuration = configuration;
            Assembly = assembly;
            RootNamespace = rootNamespace;
            Types = assembly.GetTypes().Where(TypePredicate).Select(t => new TypeMappingConfiguration(t, this)).ToList();
        }

        public string RootNamespace { get; set; }
        public Assembly Assembly { get; set; }
        internal ConfigurationBuilder Configuration { get; set; }

        public void UseThisRepository(Func<Type, IRepository<Object>> expression)
        {
            foreach (var type in Types)
            {
                var underlineType = type.UnderlineType;
                type.Repository = () => expression(underlineType);
            }
        }

        public IEnumerable<TypeMappingConfiguration> Types { get; private set; }

        public TypeMappingConfiguration For(Type type)
        {
            return Types.FirstOrDefault(configuration => configuration.UnderlineType == type);
        }
    }
}