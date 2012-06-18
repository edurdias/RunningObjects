using RunningObjects.MVC;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.RunningObjectsBootstrapper), "Start")]
namespace $rootnamespace$
{
    public static class RunningObjectsBootstrapper
    {
        public static void Start()
        {
            //ModelAssemblies.Add
            //(
            //    typeof(MyContext).Namespace, // Root Namespace
            //    Assembly.GetAssembly(typeof(MyContext)), // Assembly
            //    () => new MyContext() // Entity Framework Context
            //);

            RunningObjectsSetup.Initialize();
        }
    }
}