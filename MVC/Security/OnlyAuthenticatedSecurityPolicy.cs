namespace RunningObjects.MVC.Security
{
    public class OnlyAuthenticatedSecurityPolicy : ISecurityPolicy
    {
        public bool Authorize(SecurityPolicyContext context)
        {
            return context.IsAuthenticated;
        }
    }
}