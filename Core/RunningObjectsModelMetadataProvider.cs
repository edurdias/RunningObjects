using System;
using System.Linq;
using System.Web.Mvc;

namespace RunningObjects.Core
{
    public class RunningObjectsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        #region Overriden Methods
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            return GetMetadataForModel(modelAccessor)
                ?? base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
        }

        protected override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (containerType == null)
                throw new ArgumentNullException("containerType");

            return GetMetadataForModel(modelAccessor)
                ?? base.GetMetadataForProperty(modelAccessor, containerType, propertyDescriptor);
        }

        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            if (modelAccessor != null && (modelType == null || modelType == typeof(Object)))
            {
                var result = modelAccessor();
                if (result != null)
                    modelType = result.GetType();
            }

            if (modelType == null)
                throw new ArgumentNullException("modelType");

            return GetMetadataForModel(modelAccessor)
                   ?? base.GetMetadataForType(modelAccessor, modelType);
        }

        #endregion

        #region Private Methods
        private ModelMetadata GetMetadataForModel(Func<object> modelAccessor)
        {
            if (modelAccessor != null)
                return GetMetadataForPropertyModel(modelAccessor) ??
                       GetMetadataForParameterModel(modelAccessor);
            return null;
        }

        private ModelMetadata GetMetadataForParameterModel(Func<object> modelAccessor)
        {
            var parameter = modelAccessor() as Parameter;
            if (parameter != null)
            {
                var attributes = parameter.Descriptor.GetCustomAttributes(true).Cast<Attribute>();
                var accessor = parameter.IsModel || parameter.IsModelCollection ? modelAccessor : () => parameter.Value;
                var result = CreateMetadata(attributes, null, accessor, parameter.Descriptor.ParameterType, parameter.Descriptor.ParameterName);
                foreach (var awareAttribute in attributes.OfType<IMetadataAware>())
                    awareAttribute.OnMetadataCreated(result);
                return result;
            }
            return null;
        }

        private ModelMetadata GetMetadataForPropertyModel(Func<object> modelAccessor)
        {
            var property = modelAccessor() as Property;
            if (property != null)
            {
                var attributes = property.Descriptor.Attributes.OfType<Attribute>();
                var accessor = property.IsModel || property.IsModelCollection ? modelAccessor : () => property.Value;
                var result = CreateMetadata(attributes, null, accessor, property.MemberType, property.Name);
                foreach (var awareAttribute in attributes.OfType<IMetadataAware>())
                    awareAttribute.OnMetadataCreated(result);
                return result;
            }
            return null;
        }
        #endregion
    }
}