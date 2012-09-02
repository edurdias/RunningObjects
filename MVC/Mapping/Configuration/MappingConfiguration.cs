using System;
using System.Collections.Generic;
using System.Reflection;
using RunningObjects.MVC.Configuration;
using RunningObjects.MVC.Persistence;

namespace RunningObjects.MVC.Mapping.Configuration
{
    public class MappingConfiguration
    {
        private static readonly Dictionary<string, AssemblyMappingConfiguration> assemblies = new Dictionary<string, AssemblyMappingConfiguration>();

        public MappingConfiguration(ConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public ConfigurationBuilder Configuration { get; set; }

        public static Dictionary<string, AssemblyMappingConfiguration> Assemblies
        {
            get { return assemblies; }
        }


        public AssemblyMappingConfiguration MapAssembly(Assembly assembly, string rootNamespace)
        {
            var configuration = new AssemblyMappingConfiguration(Configuration, assembly, rootNamespace);
            assemblies.Add(rootNamespace, configuration);
            return configuration;
        }

		public AssemblyMappingConfiguration MapAssembly(Assembly assembly, string rootNamespace, Func<Type, IRepository<Object>> repositoryForAllTypes)
		{
			var configuration = MapAssembly(assembly, rootNamespace);
			configuration.UseThisRepository(repositoryForAllTypes);
			return configuration;
		}

		public AssemblyMappingConfiguration MapAssembly(Assembly assembly, string rootNamespace, Func<Type, IRepository<Object>> repositoryForAllTypes, Func<Type,bool> typeFilter)
		{
			var configuration = MapAssembly(assembly, rootNamespace, repositoryForAllTypes);
			configuration.TypeFilter = typeFilter;
			return configuration;
		}
    }
}