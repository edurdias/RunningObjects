namespace RunningObjects.MVC.Security.Containers
{
    public class ConstructorSecurityContainer<T> : SecurityPolicyContainer<T> where T : class
    {
        public ConstructorSecurityContainer(ITypeSecurityConfiguration<T> configuration)
            : base(configuration)
        {
        }
    }
}