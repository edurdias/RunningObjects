using System;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC
{
    public class MethodBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelType = ModelBinders.Binders[typeof(Type)].BindModel(controllerContext, bindingContext) as Type;
            var index = int.Parse(controllerContext.RouteData.Values["index"].ToString());
            var actionName = (string)controllerContext.RouteData.Values["action"];

            var mapping = GetMethodMapping(controllerContext, modelType, actionName, index);

            var method = new Method(new MethodDescriptor(mapping, controllerContext.GetActionDescriptor(actionName)));

            foreach (var parameter in method.Parameters)
            {
                var result = bindingContext.ValueProvider.GetValue(parameter.Name);
                if (result != null)
                {
                    if (!parameter.IsModel)
                        parameter.Value = result.ConvertTo(parameter.MemberType);
                    else
                    {
                        var context = ModelAssemblies.GetContext(parameter.MemberType);
                        var descriptor = new ModelDescriptor(ModelMappingManager.FindByType(parameter.MemberType));
                        var value = result.ConvertTo(descriptor.KeyProperty.PropertyType);
                        parameter.Value = context.Set(parameter.MemberType).Find(value);
                    }
                }
            }
            return method;
        }

        private MethodMapping GetMethodMapping(ControllerContext controllerContext, Type modelType, string actionName, int index)
        {
            var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
            switch (action)
            {
                case RunningObjectsAction.Create:
                    return GetConstructorMapping(modelType, index);
                case RunningObjectsAction.Execute:
                    return GetMethodMapping(controllerContext, modelType, index);
            }
            return null;
        }

        private static MethodMapping GetMethodMapping(ControllerContext controllerContext, Type modelType, int index)
        {
            var values = controllerContext.RouteData.Values;
            var methodName = values["methodName"].ToString();
            var typeMapping = ModelMappingManager.FindByType(modelType);
            var methods = values.ContainsKey("key") ? typeMapping.InstanceMethods : typeMapping.StaticMethods;
            var methodsOfName = methods.Where(m => m.MethodName.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));
            return methodsOfName.FirstOrDefault(m => m.Index == index);
        }

        private static MethodMapping GetConstructorMapping(Type modelType, int index)
        {
            var typeMapping = ModelMappingManager.FindByType(modelType);
            return typeMapping.Constructors.FirstOrDefault(m => m.Index == index);
        }
    }
}