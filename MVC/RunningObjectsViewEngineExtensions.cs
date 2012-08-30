using System;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC
{
    public static class RunningObjectsViewEngineExtensions
    {
        public static void UseTheme<T>(this ISecurityPolicyContainer<T> container, string name, Func<bool> expression) where T : class
        {
            RunningObjectsViewEngine.RegisterTheme(container, name, expression);
        }
    }
}
