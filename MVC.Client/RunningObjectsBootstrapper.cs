using RunningObjects.MVC.Northwind;
using RunningObjects.MVC.Persistence;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RunningObjects.MVC.Client.RunningObjectsBootstrapper), "Start")]
namespace RunningObjects.MVC.Client
{
    public static class RunningObjectsBootstrapper
    {
        public static void Start()
        {
            RunningObjectsSetup.Initialize(config =>
            {
                var referenceType = typeof(User);
                config.Mapping.MapAssembly
                (
                    referenceType.Assembly, 
                    referenceType.Namespace, 
                    type => new EntityFrameworkRepository(type, new NorthwindContext())
                );
            });
        }
    }
}