using System.Collections.Generic;
using RunningObjects.Core.Security.Policies;

namespace RunningObjects.Core.Security
{
    public class DenyRolesSecurityPolicy : AlloRolesSecurityPolicy
    {
        public DenyRolesSecurityPolicy(IEnumerable<string> roles)
            : base(roles)
        {
        }

        public override bool Authorize(SecurityPolicyContext context)
        {
            return !base.Authorize(context);
        }
    }
}