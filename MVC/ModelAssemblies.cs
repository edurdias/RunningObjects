using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Reflection;

namespace RunningObjects.MVC
{
    public static class ModelAssemblies
    {
        private static Dictionary<string, Assembly> assemblies;
        private static Dictionary<string, Func<DbContext>> contexts;

        public static Dictionary<string, Assembly> Assemblies
        {
            get { return assemblies ?? (assemblies = new Dictionary<string, Assembly>()); }
        }

        public static Dictionary<string, Func<DbContext>> Contexts
        {
            get { return contexts ?? (contexts = new Dictionary<string, Func<DbContext>>()); }
        }

        public static void Add(string @namespace, Assembly assembly, Func<DbContext> contextAccessor)
        {
            if (Assemblies.ContainsKey(@namespace))
                throw new DuplicateKeyException(@namespace, "Already exists an Assembly for specified namespace.");
            Assemblies.Add(@namespace, assembly);
            Contexts.Add(@namespace, contextAccessor);
        }

        public static void Remove(string @namespace)
        {
            if (Contains(@namespace))
            {
                Assemblies.Remove(@namespace);
                Contexts.Remove(@namespace);
            }
        }

        private static bool Contains(string @namespace)
        {
            return Assemblies.ContainsKey(@namespace) && Contexts.ContainsKey(@namespace);
        }

        public static DbContext GetContext(Type modelType)
        {
            var context = Assemblies
                .Where(asm => modelType != null && asm.Value == modelType.Assembly)
                .Select(asm => Contexts[asm.Key])
                .FirstOrDefault();

            return context != null ? context() : null;
        }
    }
}