using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RunningObjects.Core.Configuration;
using RunningObjects.Data;

namespace RunningObjects.Core.Mapping.Configuration
{
    public class AssemblyMappingConfiguration
    {
	    public string RootNamespace { get; set; }
		public Assembly Assembly { get; set; }
		public IEnumerable<TypeMappingConfiguration> Types { get; internal set; }

		internal ConfigurationBuilder Configuration { get; set; }

	    public AssemblyMappingConfiguration(ConfigurationBuilder configuration, Assembly assembly, string rootNamespace)
        {
            Configuration = configuration;
            Assembly = assembly;
            RootNamespace = rootNamespace;
            Types = assembly.GetTypes()
				.Where(t => t.IsPublic && !t.IsGenericType && !t.IsInterface && !t.IsEnum)
				.Select(t => new TypeMappingConfiguration(t, this)).ToList();

            UseThisRepository(type => new EmptyRepository<object>());
        }

        public void UseThisRepository(Func<Type, IRepository<Object>> expression)
        {
            foreach (var type in Types)
            {
                var underlineType = type.UnderlineType;
                type.Repository = () => expression(underlineType);
            }
        }

        public TypeMappingConfiguration For(Type type)
        {
            return Types.FirstOrDefault(configuration => configuration.UnderlineType == type);
        }
    }
}