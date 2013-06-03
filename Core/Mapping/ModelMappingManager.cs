using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RunningObjects.Core.Mapping.Configuration;

namespace RunningObjects.Core.Mapping
{
    public class ModelMappingManager
    {
        public const string TypeSeparator = ".";

        public static List<NamespaceMapping> Namespaces { get; private set; }
        public static Dictionary<string, TypeMapping> Types { get; set; }

        static ModelMappingManager()
        {
            Namespaces = new List<NamespaceMapping>();
            Types = new Dictionary<string, TypeMapping>();
        }

        #region Public Methods
        public static TypeMapping CurrentType(ControllerContext controllerContext)
        {
            var bindingContext = new ModelBindingContext
            {
                ModelState = controllerContext.Controller.ViewData.ModelState,
                ModelMetadata = controllerContext.Controller.ViewData.ModelMetadata,
                ModelName = TypeBinder.ModeTypeKey,
                ValueProvider = controllerContext.Controller.ValueProvider
            };

            var type = new TypeBinder().BindModel(controllerContext, bindingContext) as Type;
            if (type != null)
            {
                var mapping = FindByType(type);
                if (mapping != null)
                    return mapping;
            }
            return null;
        }

        public static IMappingElement CurrentElement(ControllerContext controllerContext)
        {
            var currentType = CurrentType(controllerContext);
            if (currentType != null)
            {
                var action = RunningObjectsAction.Welcome.GetAction(controllerContext.RouteData.Values["action"].ToString());
                switch (action)
                {
                    case RunningObjectsAction.Index:
                        var queryId = controllerContext.HttpContext.Request["q"];
                        return currentType.Queries.FirstOrDefault(q => q.ID == queryId)
                            ?? currentType.Queries.FirstOrDefault();
                    case RunningObjectsAction.Create:
                        var constructorIndex = int.Parse(controllerContext.RouteData.Values["index"].ToString());
                        return currentType.Constructors.FirstOrDefault(c => c.Index == constructorIndex);
                    case RunningObjectsAction.Execute:
                        var methodIndex = int.Parse(controllerContext.RouteData.Values["index"].ToString());
                        var methodName = controllerContext.RouteData.Values["methodName"].ToString();
                        var methods = (controllerContext.RouteData.Values.ContainsKey("key"))
                                          ? currentType.InstanceMethods
                                          : currentType.StaticMethods;
                        return methods.Where(m => m.MethodName == methodName).FirstOrDefault(m => m.Index == methodIndex);
                }
            }
            return null;
        }

        public static bool Exists(Type type)
        {
            return type != null && Types.ContainsKey(type.FullName);
        }

        private static TypeMapping FindByType(Type type)
        {
            return type != null && Exists(type) ? Types[type.FullName] : null;
        }


        public static void LoadFromConfiguration()
        {
            Namespaces.Clear();
            foreach (var configuration in MappingConfiguration.Assemblies.Values)
                LoadFromAssembly(configuration);
            ExtractTypes(Namespaces);
        }

        private static void LoadFromAssembly(AssemblyMappingConfiguration assembly)
        {
            var types = assembly.Types.Select(t => t.UnderlineType).OrderBy(t => t.Namespace).AsEnumerable();
            var ns = CreateNamespace(assembly.RootNamespace, types, assembly);
            if (ns != null)
                Namespaces.Add(ns);
        }

        private static void ExtractTypes(IEnumerable<NamespaceMapping> namespaces)
        {
            foreach (var ns in namespaces)
            {
                foreach (var type in ns.Types)
                    Types.Add(type.ModelType.FullName, type);
                ExtractTypes(ns.Namespaces);
            }
        }
        #endregion

        #region Private and Building Methods
        private static NamespaceMapping CreateNamespace(string ns, IEnumerable<Type> availableTypes, AssemblyMappingConfiguration assembly, IMappingElement parent = null)
        {
            var separator = new[] { TypeSeparator };
            const StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;

            var @namespace = new NamespaceMapping
                                 {
                                     ID = Guid.NewGuid().ToString("N"),
                                     Name = ns.Split(separator, options).LastOrDefault(),
                                     FullName = ns,
                                     Parent = parent,
                                     Visible = true
                                 };

            @namespace.Types = availableTypes
                .Where(t => t.Namespace == ns)
                .Select(t => CreateTypeMapping(t, @namespace, assembly.For(t)))
                .ToList();

            @namespace.Namespaces = availableTypes
                .Where(t => t.Namespace != null && t.Namespace != ns && t.Namespace.StartsWith(ns))
                .GroupBy(t => t.Namespace)
                .Select(g => g.Key.Replace(ns + TypeSeparator, string.Empty).Split(separator, options).FirstOrDefault())
                .Select(nm => CreateNamespace(string.Concat(ns, TypeSeparator, nm), availableTypes.Where(t => t.Namespace != ns), assembly, @namespace))
                .ToList();

            return @namespace;
        }

        private static TypeMapping CreateTypeMapping(Type type, NamespaceMapping @namespace, TypeMappingConfiguration configuration)
        {
	        var mapping = new TypeMapping
	        {
		        ID = Guid.NewGuid().ToString("N"),
		        ModelType = type,
		        Namespace = @namespace,
		        Configuration = configuration
	        };

            var attributes = type.GetCustomAttributes(true);

            var displayName = attributes.OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
	        mapping.Name = (displayName != null && !string.IsNullOrEmpty(displayName.DisplayName))
		                       ? displayName.DisplayName
		                       : type.Name;

            var scaffold = attributes.OfType<ScaffoldTableAttribute>().FirstOrDefault();
            mapping.Visible = (scaffold == null || scaffold.Scaffold) && !type.GetCustomAttributes(false).OfType<ScriptOnlyAttribute>().Any();

            var properties = OrderProperties(type, TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>());

            var ignoredNames = new List<string>
            {
                "Equals",
                "GetHashCode",
                "ToString",
                "GetType",
                "ReferenceEquals"
            };

            foreach (var property in properties)
            {
                ignoredNames.Add("get_" + property.Name);
                ignoredNames.Add("set_" + property.Name);
            }

            mapping.Properties = properties.Where(p => !p.Attributes.OfType<ScriptOnlyAttribute>().Any());
            mapping.Key = properties.FirstOrDefault(p => p.Attributes.OfType<KeyAttribute>().Any());
            mapping.Text = properties.FirstOrDefault(p => p.Attributes.OfType<TextAttribute>().Any());

            var constructors = type.GetConstructors()
                .Where(ctor => ctor.IsPublic && ctor.GetParameters().Any())
                .ToList();

            mapping.Constructors = constructors.Select(m => CreateMethodMapping(mapping, m, constructors.IndexOf(m))).ToList();

            mapping.StaticMethods = type.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.IsStatic && m.IsPublic && !ignoredNames.Any(nm => nm == m.Name))
                .GroupBy(m => m.Name)
                .SelectMany(g => g.Select(m => CreateMethodMapping(mapping, m, g.ToList().IndexOf(m))))
                .ToList();

            mapping.InstanceMethods = type.GetMethods()
                .Where(m => !m.IsStatic && m.IsPublic && !ignoredNames.Any(nm => nm == m.Name))
                .GroupBy(m => m.Name)
                .SelectMany(g => g.Select(m => CreateMethodMapping(mapping, m, g.ToList().IndexOf(m))))
                .ToList();

            var queryAttributes = type.GetCustomAttributes(false).OfType<QueryAttribute>();
            if (queryAttributes.Any())
            {
                mapping.Queries = queryAttributes
                   .OrderBy(q => q.Name)
                   .Select(q => CreateQueryMapping(q, mapping))
                   .ToList();
            }
            else
                mapping.Queries = new[] { new QueryAttribute() }.Select(q => CreateQueryMapping(q, mapping)).ToList();

            return mapping;
        }

        private static IEnumerable<PropertyDescriptor> OrderProperties(Type componentType, IEnumerable<PropertyDescriptor> properties)
        {
            var componentTypes = new List<Type>();
            do
            {
                componentTypes.Add(componentType);
                componentType = componentType.BaseType;
            } while (componentType != null && (componentType.BaseType != null || componentType.BaseType != typeof(object)));
            componentTypes.Reverse();
            return componentTypes.SelectMany(ct => properties.Where(pd => pd.ComponentType == ct));
        }

        private static MethodMapping CreateMethodMapping(TypeMapping type, MethodBase method, int index)
        {
            var display = method.GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault();
            var displayName = method.GetCustomAttributes(false).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            var name = display == null
                           ? displayName == null ? method.IsConstructor ? "New" : method.Name : displayName.DisplayName
                           : display.GetName();

            return new MethodMapping
                       {
                           ID = Guid.NewGuid().ToString("N"),
                           Name = name,
                           MethodName = method.Name,
                           Parameters = method.GetParameters(),
                           Index = index,
                           Type = type,
                           ReturnType = method is ConstructorInfo ? typeof(void) : ((MethodInfo)method).ReturnType,
                           UnderlineAction = method.IsConstructor ? RunningObjectsAction.Create : RunningObjectsAction.Execute,
                           Visible = !ScriptOnlyAttribute.Contains(method),
                           Method = method
                       };
        }

        private static QueryMapping CreateQueryMapping(QueryAttribute query, TypeMapping type)
        {
            return new QueryMapping
            {
                ID = query.Id ?? Guid.NewGuid().ToString("N"),
                Name = string.IsNullOrEmpty(query.Name) ? "All Items" : query.Name,
                Type = type,
                Visible = true
            };
        }
        #endregion

        public static TypeMapping MappingFor(Type type)
        {
            var mapping = FindByType(type);
            if (mapping != null)
                return mapping;

            var asms = MappingConfiguration.Assemblies.Values;
            var configuration = asms.SelectMany(asm => asm.Types).FirstOrDefault(config => config.UnderlineType == type);
            return CreateTypeMapping(type, null, configuration);
        }
    }
}