using System.Collections.Generic;

namespace RunningObjects.MVC.Security
{
    public abstract class SecurityPolicyContainer<T> : ISecurityPolicyContainer<T>
    {
        private readonly List<ISecurityPolicy> policies = new List<ISecurityPolicy>();

        public List<ISecurityPolicy> Policies { get { return policies; } }
    }
}