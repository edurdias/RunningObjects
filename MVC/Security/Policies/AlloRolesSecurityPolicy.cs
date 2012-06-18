using System.Collections.Generic;
using System.Linq;

namespace RunningObjects.MVC.Security.Policies
{
    public class AlloRolesSecurityPolicy : ISecurityPolicy
    {
        public AlloRolesSecurityPolicy(IEnumerable<string> roles)
        {
            Roles = roles;
        }

        protected IEnumerable<string> Roles { get; set; }

        #region Implementation of ISecurityPolicy

        public virtual bool Authorize(SecurityPolicyContext context)
        {
            return context.CurrentUserRoles().Any(role => Roles.Contains(role));
        }

        #endregion
    }
}