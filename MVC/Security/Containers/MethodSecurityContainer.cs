using System.Reflection;

namespace RunningObjects.MVC.Security.Containers
{
    public class MethodSecurityContainer<T> : SecurityPolicyContainer<T>
    {
        public MethodSecurityContainer(MethodInfo method)
        {
            Method = method;
        }

        public MethodInfo Method { get; set; }
    }
}