using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace RunningObjects.MVC.Mapping
{
    public class ModelMappingManager
    {
        public const string TypeSeparator = ".";

        public static List<NamespaceMapping> Namespaces { get; private set; }
        private static Dictionary<string, TypeMapping> Types { get; set; }

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

        public static IElementMapping CurrentElement(ControllerContext controllerContext)
        {
            var currentType = CurrentType(controllerContext);
            if (currentType != null)
            {
                var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), controllerContext.RouteData.Values["action"].ToString(), true);
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

        public static TypeMapping FindByType(Type type)
        {
            return type != null && Exists(type) ? Types[type.FullName] : null;
        }


        public static void LoadFrom(Dictionary<string, Assembly> assemblies)
        {
            Namespaces.Clear();
            foreach (var rootNamespace in assemblies.Keys)
            {
                var assembly = assemblies[rootNamespace];
                LoadFromAssembly(assembly, rootNamespace);
            }
            ExtractTypes(Namespaces);
        }

        private static void LoadFromAssembly(Assembly assembly, string rootNamespace)
        {
            var types = assembly.GetTypes().Where(t => t.IsPublic && !t.IsGenericType && !t.IsInterface && !t.IsSubclassOf(typeof(DbContext))).OrderBy(t => t.Namespace);
            var ns = CreateNamespace(rootNamespace, types);
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
        private static NamespaceMapping CreateNamespace(string ns, IEnumerable<Type> availableTypes, NamespaceMapping parent = null)
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
                .Select(t => CreateTypeMapping(t, @namespace))
                .ToList();

            @namespace.Namespaces = availableTypes
                .Where(t => t.Namespace != null && t.Namespace != ns && t.Namespace.StartsWith(ns))
                .GroupBy(t => t.Namespace)
                .Select(g => g.Key.Replace(ns + TypeSeparator, string.Empty).Split(separator, options).FirstOrDefault())
                .Select(nm => CreateNamespace(string.Concat(ns, TypeSeparator, nm), availableTypes.Where(t => t.Namespace != ns), @namespace))
                .ToList();

            return @namespace;
        }

        private static TypeMapping CreateTypeMapping(Type type, NamespaceMapping @namespace)
        {
            var mapping = new TypeMapping
                              {
                                  ID = Guid.NewGuid().ToString("N"),
                                  ModelType = type,
                                  Namespace = @namespace
                              };

            var attributes = type.GetCustomAttributes(true);

            var displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            mapping.Name = (displayName != null && !string.IsNullOrEmpty(displayName.DisplayName))
                               ? displayName.DisplayName
                               : type.Name;

            var scaffold = attributes.OfType<ScaffoldTableAttribute>().FirstOrDefault();
            mapping.Visible = scaffold == null || scaffold.Scaffold;

            var properties = TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>();

            mapping.Properties = properties;
            mapping.Key = properties.FirstOrDefault(p => p.Attributes.OfType<KeyAttribute>().Any());
            mapping.Text = properties.FirstOrDefault(p => p.Attributes.OfType<TextAttribute>().Any());


            var ignoredNames = new List<string>
                {
                    "Equals",
                    "GetHashCode",
                    "ToString",
                    "GetType"
                };
            foreach (var property in properties)
            {
                ignoredNames.Add("get_" + property.Name);
                ignoredNames.Add("set_" + property.Name);
            }

            var constructors = type.GetConstructors()
                .Where(ctor => ctor.IsPublic && ctor.GetParameters().Any())
                .ToList();

            mapping.Constructors = constructors.Select(m => CreateMethodMapping(mapping, m, constructors.IndexOf(m))).ToList();

            mapping.StaticMethods = type.GetMethods()
                .Where(m => m.IsStatic && m.IsPublic)
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
                mapping.Queries = new[] {new QueryAttribute()}.Select(q => CreateQueryMapping(q, mapping)).ToList();

            return mapping;
        }

        private static MethodMapping CreateMethodMapping(TypeMapping type, MethodBase method, int index)
        {
            var display = method.GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault();

            return new MethodMapping
                       {
                           ID = Guid.NewGuid().ToString("N"),
                           //TODO:Create resource for string value
                           Name = display == null ? method.IsConstructor ? "New" : method.Name : display.GetName(),
                           MethodName = method.Name,
                           Parameters = method.GetParameters(),
                           Index = index,
                           Type = type,
                           ReturnType = method is ConstructorInfo ? typeof(void) : ((MethodInfo)method).ReturnType,
                           UnderlineAction = method.IsConstructor ? RunningObjectsAction.Create : RunningObjectsAction.Execute,
                           Visible = true,
                           Method = method
                       };
        }

        private static QueryMapping CreateQueryMapping(QueryAttribute query, TypeMapping type)
        {
            return new QueryMapping
            {
                ID = query.Id ?? Guid.NewGuid().ToString("N"),
                //TODO:Create resource for string value
                Name = string.IsNullOrEmpty(query.Name) ? "All Items" : query.Name,
                Type = type,
                Visible = true
            };
        }
        #endregion

        public static TypeMapping MappingFor(Type type)
        {
            return CreateTypeMapping(type, null);
        }
    }
}