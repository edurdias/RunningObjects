using RunningObjects.MVC.Security.Policies;

namespace RunningObjects.MVC.Security
{
    public static class SecurityPolicyContainerExtensions
    {
        public static ITypeSecurityConfiguration<T> AllowAll<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new AllowAllSecurityPolicy());
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> AllowRoles<T>(this ISecurityPolicyContainer<T> container, params string[] roles)
        {
            container.Policies.Add(new AlloRolesSecurityPolicy(roles));
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> DenyAll<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new DenyAllSecurityPolicy());
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> DenyRoles<T>(this ISecurityPolicyContainer<T> container, params string[] roles)
        {
            container.Policies.Add(new DenyRolesSecurityPolicy(roles));
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> ApplyCustomPolicy<T>(this ISecurityPolicyContainer<T> container, ISecurityPolicy policy)
        {
            container.Policies.Add(policy);
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> OnlyAnonymous<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new OnlyAnonymousSecurityPolicy());
            return container.Configuration();
        }

        public static ITypeSecurityConfiguration<T> OnlyAuthenticated<T>(this ISecurityPolicyContainer<T> container)
        {
            container.Policies.Add(new OnlyAuthenticatedSecurityPolicy());
            return container.Configuration();
        } 
    }
}