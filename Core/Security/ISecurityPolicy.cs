namespace RunningObjects.Core.Security
{
    public interface ISecurityPolicy
    {
        bool Authorize(SecurityPolicyContext context);
    }
}