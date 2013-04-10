namespace RunningObjects.Core.Security
{
    public interface ITypeSecurityConfigurationBuilder<T> 
    {
        void Configure(ITypeSecurityConfiguration<T> configuration);
    }
}