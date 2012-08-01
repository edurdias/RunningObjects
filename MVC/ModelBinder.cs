using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC
{
    public class ModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelType = ModelBinders.Binders[typeof(Type)].BindModel(controllerContext, bindingContext) as Type;

            var key = controllerContext.RouteData.Values["key"];
            var mapping = ModelMappingManager.MappingFor(modelType);
            var descriptor = new ModelDescriptor(mapping);

            using (var repository = mapping.Configuration.Repository())
            {
                var keyValues = descriptor.KeyProperty.PropertyType == typeof (Guid)
                    ? Guid.Parse(key.ToString())
                    : Convert.ChangeType(key, descriptor.KeyProperty.PropertyType);
                var instance = repository.Find(keyValues);

                var model = new Model(modelType, descriptor, instance);
                foreach (var property in model.Properties)
                {
                    var result = bindingContext.ValueProvider.GetValue(property.Name);
                    if (result != null)
                    {
                        if (!property.IsModel)
                        {
                            var converter = TypeDescriptor.GetConverter(property.MemberType);
                            var value = property.MemberType == typeof (Boolean)
                                            ? result.AttemptedValue.Split(',')[0]
                                            : result.AttemptedValue;

                            property.Value = converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        }
                        else
                        {
                            var propertyTypeMapping = ModelMappingManager.MappingFor(property.MemberType);
                            var propertyDescriptor = new ModelDescriptor(propertyTypeMapping);
                            var propertyValue = result.ConvertTo(propertyDescriptor.KeyProperty.PropertyType);
                            property.Value = repository.Find(propertyValue);
                        }
                    }
                }
                //TODO:Check detaching with Repository pattern
                //context.Entry(model.Instance).State = EntityState.Detached;
                return model; 
            }
        }
    }
}