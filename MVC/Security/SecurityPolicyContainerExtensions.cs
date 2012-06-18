using RunningObjects.MVC.Security.Policies;

namespace RunningObjects.MVC.Security
{
    public static class SecurityPolicyContainerExtensions
    {
        public static ISecurityPolicyContainer<T> AllowAll<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new AllowAllSecurityPolicy());
            return container;
        }

        public static ISecurityPolicyContainer<T> AllowRoles<T>(this ISecurityPolicyContainer<T> container, params string[] roles)
        {
            container.Policies.Add(new AlloRolesSecurityPolicy(roles));
            return container;
        }

        public static ISecurityPolicyContainer<T> DenyAll<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new DenyAllSecurityPolicy());
            return container;
        }

        public static ISecurityPolicyContainer<T> DenyRoles<T>(this ISecurityPolicyContainer<T> container, params string[] roles)
        {
            container.Policies.Add(new DenyRolesSecurityPolicy(roles));
            return container;
        }

        public static ISecurityPolicyContainer<T> ApplyCustomPolicy<T>(this ISecurityPolicyContainer<T> container, ISecurityPolicy policy)
        {
            container.Policies.Add(policy);
            return container;
        }
    }
}