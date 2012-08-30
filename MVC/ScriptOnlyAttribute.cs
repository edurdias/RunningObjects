using System;
using System.Linq;
using System.Reflection;

namespace RunningObjects.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Method)]
    public class ScriptOnlyAttribute : Attribute
    {
        public static bool Contains(MethodBase method)
        {
            return method.GetCustomAttributes(true).OfType<ScriptOnlyAttribute>().Any();
        }
    }
}