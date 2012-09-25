using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC
{
    public class MethodBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var actionName = (string)controllerContext.RouteData.Values["action"];
            var mapping = GetMethodMapping(controllerContext, bindingContext, actionName);

            var method = new Method(new MethodDescriptor(mapping, controllerContext.GetActionDescriptor(actionName)));

            foreach (var parameter in method.Parameters)
            {
                ValueProviderResult result;
                if (parameter.IsModelCollection)
                {
                    var type = typeof(List<>).MakeGenericType(parameter.UnderliningModel.ModelType);
                    var collection = Activator.CreateInstance(type);
                    var index = 0;
                    while ((result = bindingContext.ValueProvider.GetValue(string.Format("{0}[{1}]", parameter.Name, index))) != null)
                    {
                        var item = ModelBinder.GetModelValue(result, parameter.UnderliningModel.ModelType);
                        var itemModel = new Model(parameter.UnderliningModel.ModelType, parameter.UnderliningModel.Descriptor, item);
                        foreach (var itemProperty in itemModel.Properties)
                        {
                            var itemPropertyName = string.Format("{0}[{1}].{2}", parameter.Name, index, itemProperty.Name);
                            var itemResult = bindingContext.ValueProvider.GetValue(itemPropertyName);
                            if (itemResult != null)
                            {
                                itemProperty.Value = !itemProperty.IsModel
                                    ? ModelBinder.GetNonModelValue(itemResult, itemProperty.MemberType)
                                    : ModelBinder.GetModelValue(itemResult, itemProperty.MemberType);
                            }
                        }
                        type.GetMethod("Add").Invoke(collection, new[] { item });
                        index++;
                    }
                    parameter.Value = collection;
                }
                else
                {
                    result = bindingContext.ValueProvider.GetValue(parameter.Name);
                    if (result != null)
                    {
                        parameter.Value = !parameter.IsModel
                            ? ModelBinder.GetNonModelValue(result, parameter.MemberType)
                            : ModelBinder.GetModelValue(result, parameter.MemberType);
                    }
                }
            }
            return method;
        }

        

        public static MethodMapping GetMethodMapping(ControllerContext controllerContext, ModelBindingContext bindingContext, string actionName)
        {
            var modelType = ModelBinders.Binders[typeof(Type)].BindModel(controllerContext, bindingContext) as Type;
            var index = int.Parse(controllerContext.RouteData.Values["index"].ToString());
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
            var typeMapping = ModelMappingManager.MappingFor(modelType);
            var methods = values.ContainsKey("key") ? typeMapping.InstanceMethods : typeMapping.StaticMethods;
            var methodsOfName = methods.Where(m => m.MethodName.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));
            return methodsOfName.FirstOrDefault(m => m.Index == index);
        }

        private static MethodMapping GetConstructorMapping(Type modelType, int index)
        {
            var typeMapping = ModelMappingManager.MappingFor(modelType);
            return typeMapping.Constructors.FirstOrDefault(m => m.Index == index);
        }

        public static ModelBindingContext CreateBindingContext(ControllerContext controllerContext)
        {
            return new ModelBindingContext
            {
                ModelState = controllerContext.Controller.ViewData.ModelState,
                ModelMetadata = controllerContext.Controller.ViewData.ModelMetadata,
                ModelName = "methodName",
                ValueProvider = controllerContext.Controller.ValueProvider
            };
        }
    }
}