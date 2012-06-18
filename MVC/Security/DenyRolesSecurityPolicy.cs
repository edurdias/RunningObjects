using System.Collections.Generic;
using RunningObjects.MVC.Security.Policies;

namespace RunningObjects.MVC.Security
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