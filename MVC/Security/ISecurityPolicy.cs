namespace RunningObjects.MVC.Security
{
    public interface ISecurityPolicy
    {
        bool Authorize(SecurityPolicyContext context);
    }
}