namespace RunningObjects.MVC.Security
{
    public interface ITypeSecurityConfigurationBuilder<T> 
    {
        void Configure(ITypeSecurityConfiguration<T> configuration);
    }
}