using System.Collections.Generic;

namespace RunningObjects.MVC.Security
{
    public interface ISecurityPolicyContainer<out T> 
    {
        List<ISecurityPolicy> Policies { get; }
        ITypeSecurityConfiguration<T> Configuration();
    }
}