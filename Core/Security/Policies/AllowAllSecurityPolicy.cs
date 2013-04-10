namespace RunningObjects.Core.Security.Policies
{
    public class AllowAllSecurityPolicy : ISecurityPolicy
    {
        #region Implementation of ISecurityPolicy

        public bool Authorize(SecurityPolicyContext context)
        {
            return true;
        }

        #endregion
    }
}