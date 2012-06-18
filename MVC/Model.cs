using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RunningObjects.MVC
{
    public class Model
    {
        private readonly List<Property> properties = new List<Property>();

        public Model(Type modelType, ModelDescriptor descriptor, object instance = null)
        {
            if (modelType == null)
                throw new ArgumentNullException("modelType");
            if (descriptor == null)
                throw new ArgumentNullException("descriptor");

            ModelType = modelType;
            Descriptor = descriptor;
            Instance = instance;

            properties.AddRange(descriptor.Properties.Select(p => p.AsModel(this)));
        }

        public Type ModelType { get; set; }

        public ModelDescriptor Descriptor { get; set; }

        public object Instance { get; set; }

        public IEnumerable<Property> Properties
        {
            get { return properties; }
        }

        public PropertyDescriptor Key
        {
            get { return Descriptor.KeyProperty; }
        }

        public object KeyValue { get { return Key != null ? Key.GetValue(Instance) : null; } }

        public PropertyDescriptor Text
        {
            get { return Descriptor.TextProperty; }
        }

        public object TextValue { get { return Text != null ? Text.GetValue(Instance) : null; } }
    }
}