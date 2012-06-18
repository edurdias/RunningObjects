using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningObjects.MVC
{
    public static class TypeExtensions
    {
        public static string PartialName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            var namespaces = new List<string>(ModelAssemblies.Assemblies.Keys);
            var typeNamespace = namespaces.FirstOrDefault(ns => !string.IsNullOrEmpty(type.FullName) && type.FullName.StartsWith(ns));

            return !string.IsNullOrEmpty(type.FullName) && !string.IsNullOrEmpty(typeNamespace)
                       ? type.FullName.Replace(string.Format("{0}.", typeNamespace), string.Empty)
                       : type.FullName;
        }
    }
}