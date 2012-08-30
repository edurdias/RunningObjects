using System.Reflection;

namespace RunningObjects.MVC.Security.Containers
{
    public class MethodSecurityContainer<T> : SecurityPolicyContainer<T> where T : class
    {
        public MethodSecurityContainer(ITypeSecurityConfiguration<T> configuration, MethodInfo method)
            : base(configuration)
        {
            Method = method;
        }

        public MethodInfo Method { get; set; }
    }
}