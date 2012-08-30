namespace RunningObjects.MVC.Security.Containers
{
    public class AnythingSecurityContainer<T> : SecurityPolicyContainer<T> where T : class
    {
        public AnythingSecurityContainer(ITypeSecurityConfiguration<T> configuration)
            : base(configuration)
        {
        }
    }
}
