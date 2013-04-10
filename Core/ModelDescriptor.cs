using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RunningObjects.Core.Mapping;

namespace RunningObjects.Core
{
    public class ModelDescriptor : ICustomTypeDescriptor
    {
        private AttributeCollection attributes;
        private string className;
        private string componentName;
        private TypeConverter converter;
        private EventDescriptor defaultEvent;
        private PropertyDescriptor keyProperty;
        private PropertyDescriptor textProperty;

        public ModelDescriptor(TypeMapping mapping)
        {
            ModelMapping = mapping;
        }

        public TypeMapping ModelMapping { get; set; }

        public Type ModelType
        {
            get { return ModelMapping.ModelType; }
        }

        public IEnumerable<PropertyDescriptor> Properties
        {
            get { return ModelMapping.Properties; }
        }

        public PropertyDescriptor KeyProperty
        {
            get { return keyProperty ?? (keyProperty = Properties.FirstOrDefault(p => p.Attributes.OfType<KeyAttribute>().Any())); }
        }

        public PropertyDescriptor TextProperty
        {
            get { return textProperty ?? (textProperty = Properties.FirstOrDefault(p => p.Attributes.OfType<TextAttribute>().Any())); }
        }

        #region ICustomTypeDescriptor methods
        public AttributeCollection GetAttributes()
        {
            return attributes ?? (attributes = TypeDescriptor.GetAttributes(ModelType));
        }

        public string GetClassName()
        {
            if (string.IsNullOrEmpty(className))
                className = TypeDescriptor.GetClassName(ModelType);
            return className;
        }

        public string GetComponentName()
        {
            if (string.IsNullOrEmpty(componentName))
                componentName = TypeDescriptor.GetComponentName(ModelType);
            return componentName;
        }

        public TypeConverter GetConverter()
        {
            return converter ?? (converter = TypeDescriptor.GetConverter(ModelType));
        }

        public EventDescriptor GetDefaultEvent()
        {
            return defaultEvent ?? (defaultEvent = TypeDescriptor.GetDefaultEvent(ModelType));
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TextProperty;
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(ModelType, editorBaseType);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(ModelType);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attrs)
        {
            return TypeDescriptor.GetEvents(ModelType, attrs);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(ModelType);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attrs)
        {
            return TypeDescriptor.GetProperties(ModelType, attrs);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return GetProperties().Contains(pd) ? this : null;
        }
        #endregion
    }
}