using System.Collections.Generic;

namespace RunningObjects.Core.Security
{
    public abstract class SecurityPolicyContainer<T> : ISecurityPolicyContainer<T> where T : class
    {
        private readonly List<ISecurityPolicy> policies = new List<ISecurityPolicy>();
        private readonly ITypeSecurityConfiguration<T> configuration; 

        protected SecurityPolicyContainer(ITypeSecurityConfiguration<T> configuration)
        {
            this.configuration = configuration;
        }

        public List<ISecurityPolicy> Policies { get { return policies; } }

        public ITypeSecurityConfiguration<T> Configuration()
        {
            return configuration;
        }
    }
}