using System;
using System.Collections.Generic;
using System.Linq;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Persistence;

namespace RunningObjects.MVC
{
    public static class TypeExtensions
    {
        public static string PartialName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var namespaces = new List<string>(ModelMappingManager.Namespaces.Select(ns => ns.FullName));
            var typeNamespace = namespaces.FirstOrDefault(ns => !string.IsNullOrEmpty(type.FullName) && type.FullName.StartsWith(ns));

            return !string.IsNullOrEmpty(type.FullName) && !string.IsNullOrEmpty(typeNamespace)
                       ? type.FullName.Replace(string.Format("{0}.", typeNamespace), string.Empty)
                       : type.FullName;
        }

        public static IRepository<object> Repository(this Type type)
        {
            var mapping = ModelMappingManager.MappingFor(type);
            return mapping != null ? mapping.Configuration.Repository() : null;
        }

        public static IRepository<T> Repository<T>(this Type type) where T : class
        {
            var mapping = ModelMappingManager.MappingFor(type);
            if (mapping != null)
                return mapping.Configuration.Repository() as IRepository<T>;
            return null;
        }

        public static IRepository<object> Repository(this Object instance)
        {
            return instance != null ? instance.GetType().Repository() : null;
        }
    }
}