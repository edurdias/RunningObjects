using System;
using System.Collections.Generic;
using System.Linq;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC
{
    public static class TypeExtensions
    {
        public static string PartialName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var namespaces = new List<string>(ModelMappingManager.Namespaces.Select(ns=> ns.FullName));
            var typeNamespace = namespaces.FirstOrDefault(ns => !string.IsNullOrEmpty(type.FullName) && type.FullName.StartsWith(ns));

            return !string.IsNullOrEmpty(type.FullName) && !string.IsNullOrEmpty(typeNamespace)
                       ? type.FullName.Replace(string.Format("{0}.", typeNamespace), string.Empty)
                       : type.FullName;
        }
    }
}