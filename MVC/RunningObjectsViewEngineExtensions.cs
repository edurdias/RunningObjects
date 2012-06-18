using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC
{
    public static class RunningObjectsViewEngineExtensions
    {
        public static void UseThemeWhen<T>(this ISecurityPolicyContainer<T> container, string theme, Func<bool> expression)
        {
            RunningObjectsViewEngine.RegisterTheme<T>(container, theme, expression);
        }
    }
}
