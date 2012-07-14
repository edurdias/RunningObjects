using RunningObjects.MVC;
using RunningObjects.MVC.Persistence;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.RunningObjectsBootstrapper), "Start")]
namespace $rootnamespace$
{
    public static class RunningObjectsBootstrapper
    {
        public static void Start()
        {
            RunningObjectsSetup.Initialize(config =>
            {
                //var referenceType = typeof(MyDomainClass);
                //config.Mapping.MapAssembly
                //(
                //    referenceType.Assembly, 
                //    referenceType.Namespace, 
                //    type => new EntityFrameworkRepository(type, new MyDbContext())
                //);
            });
        }
    }
}