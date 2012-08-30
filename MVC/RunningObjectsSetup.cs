using System;
using System.Web.Mvc;
using System.Web.Routing;
using RunningObjects.MVC.Configuration;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Security;

namespace RunningObjects.MVC
{
    public sealed class RunningObjectsSetup
    {
        private static readonly ConfigurationBuilder configuration = new ConfigurationBuilder();

        public static ConfigurationBuilder Configuration
        {
            get { return configuration; }
        }
        public static void Initialize(Action<ConfigurationBuilder> expression)
        {
            InitializeControllerFactory();
            InitializeViewEngine();
            InitializeBinders();
            InitializeMetadataProvider();
            InitializeValidation();
            InitializeRoutes();
            expression(Configuration);
            InitializeSecurity(Configuration.Security);
            InitializeMapping();
        }

        

        #region Initialization Steps

        private static void InitializeControllerFactory()
        {
            ControllerBuilder.Current.DefaultNamespaces.Add(typeof(Controllers.ControllerBase).Namespace);
        }

        private static void InitializeSecurity(SecurityConfigurationBuilder securityConfiguration)
        {
            GlobalFilters.Filters.Add(new SecurityHandlerAttribute(securityConfiguration), 0);
        }

        private static void InitializeMapping()
        {
            ModelMappingManager.LoadFromConfiguration();
        }

        private static void InitializeValidation()
        {
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new RunningObjectsValidatorProvider());
            ModelValidatorProviders.Providers.Add(new DataErrorInfoModelValidatorProvider());
            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
        }

        private static void InitializeMetadataProvider()
        {
            ModelMetadataProviders.Current = new RunningObjectsModelMetadataProvider();
        }

        private static void InitializeBinders()
        {
            ModelBinders.Binders.Add(typeof(Type), new TypeBinder());
            ModelBinders.Binders.Add(typeof(Method), new MethodBinder());
            ModelBinders.Binders.Add(typeof(Model), new ModelBinder());
        }

        private static void InitializeViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RunningObjectsViewEngine());
        }

        private static void InitializeRoutes()
        {
            var routes = RouteTable.Routes;

            #region Running Objects Default Service Controller
            routes.MapRoute(
                "RO - Service - All",
                "api/{modelType}",
                new { controller = "Service", action = "Index" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );
            routes.MapRoute(
                "RO - Service - Create",
                "api/{modelType}",
                new { controller = "Service", action = "Create", index = 0 },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "RO - Service - View",
                "api/{modelType}/{key}",
                new { controller = "Service", action = "View" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "RO - Service - Edit",
                "api/{modelType}/{key}",
                new { controller = "Service", action = "Edit" },
                new { httpMethod = new HttpMethodConstraint("PUT") }
                );

            routes.MapRoute(
                "RO - Service - Delete",
                "api/{modelType}/{key}",
                new { controller = "Service", action = "Delete" },
                new { httpMethod = new HttpMethodConstraint("DELETE") }
                );
            routes.MapRoute(
                "RO - Service - Execute Method",
                "api/{modelType}/{key}/{methodName}/{index}",
                new { controller = "Service", action = "Execute", index = 0 },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "RO - Service - Execute Static Method",
                "api/{modelType}/{methodName}/{index}",
                new { controller = "Service", action = "Execute", index = 0 },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );
            #endregion

            #region Running Objects Default Presentation Controller
            routes.MapRoute(
                "Running Objects - All",
                "{modelType}",
                new { controller = "Presentation", action = "Index" }
                );

            routes.MapRoute(
                "Running Objects - Welcome",
                "",
                new { controller = "Presentation", action = "Welcome" }
                );

            routes.MapRoute(
                "Running Objects - Create",
                "{modelType}/Create/{index}",
                new { controller = "Presentation", action = "Create", index = 0 }
                );

            routes.MapRoute(
                "Running Objects - View / Edit / Delete",
                "{modelType}/{key}/{action}",
                new { controller = "Presentation" },
                new { actionNames = new ActionNameConstraint("View", "Edit", "Delete") }
                );

            routes.MapRoute(
                "Running Objects - Execute Method",
                "{modelType}/{key}/{methodName}/{index}",
                new { controller = "Presentation", action = "Execute", index = 0 }
                );

            routes.MapRoute(
                "Running Objects - Execute Static Method",
                "{modelType}/{methodName}/{index}",
                new { controller = "Presentation", action = "Execute", index = 0 }
                );
            #endregion
        }
        #endregion
    }
}