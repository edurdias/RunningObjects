using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RunningObjects.MVC.Client.RunningObjectsBootstrapper), "Start")]
namespace RunningObjects.MVC.Client
{
    public static class RunningObjectsBootstrapper
    {
        public static void Start()
        {
            //ModelAssemblies.Add
            //(
            //    typeof(NorthwindContext).Namespace, // Root Namespace
            //    Assembly.GetAssembly(typeof(NorthwindContext)), // Assembly
            //    () => new NorthwindContext() // Entity Framework Context
            //);

            RunningObjectsSetup.Initialize();

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");
        }
    }
}