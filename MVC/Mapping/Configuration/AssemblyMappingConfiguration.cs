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
        public static readonly Func<Type, bool> TypePredicate = t => t.IsPublic && !t.IsGenericType && !t.IsInterface && !t.IsSubclassOf(typeof(DbContext)) && !t.IsEnum;

	    public Func<Type, bool> TypeFilter { get; set; }

	    public AssemblyMappingConfiguration(ConfigurationBuilder configuration, Assembly assembly, string rootNamespace)
        {
            Configuration = configuration;
            Assembly = assembly;
            RootNamespace = rootNamespace;
            Types = assembly.GetTypes().Where(TypePredicate).Select(t => new TypeMappingConfiguration(t, this)).ToList();
            UseThisRepository(type => new EmptyRepository<object>());
        }

		public AssemblyMappingConfiguration(ConfigurationBuilder configuration, Assembly assembly, string rootNamespace, Func<Type, bool> typeFilter)
			: this(configuration, assembly, rootNamespace)
		{
			TypeFilter = typeFilter;
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

	    public IEnumerable<Type> FilterTypes(IEnumerable<Type> types)
	    {
			if (TypeFilter == null)
				return types;

		    return types.Where(t => TypeFilter(t));
	    }
    }
}