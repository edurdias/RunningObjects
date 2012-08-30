namespace RunningObjects.MVC.Security
{
    public class OnlyAnonymousSecurityPolicy : ISecurityPolicy
    {
        public bool Authorize(SecurityPolicyContext context)
        {
            return !context.IsAuthenticated;
        }
    }
}