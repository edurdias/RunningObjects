using System.Collections.Generic;

namespace RunningObjects.Core.Security
{
    public interface ISecurityPolicyContainer<out T> 
    {
        List<ISecurityPolicy> Policies { get; }
        ITypeSecurityConfiguration<T> Configuration();
    }
}