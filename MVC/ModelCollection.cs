using System;
using System.Collections;
using System.Collections.Generic;

namespace RunningObjects.MVC
{
    public class ModelCollection : IPagedCollection
    {
        private readonly List<Model> items = new List<Model>();
        private readonly List<Property> properties = new List<Property>();

        public ModelCollection(Type modelType, ModelDescriptor descriptor, IEnumerable items)
        {
            if (modelType == null)
                throw new ArgumentNullException("modelType");
            if (descriptor == null)
                throw new ArgumentNullException("descriptor");
            ModelType = modelType;
            Descriptor = descriptor;

            if (items != null)
                foreach (var item in items)
                    this.items.Add(new Model(modelType, descriptor, item));

            var emptyModel = new Model(modelType, descriptor);

            foreach (var property in descriptor.Properties)
                properties.Add(property.AsModel(emptyModel));
        }

        public Type ModelType { get; set; }

        public ModelDescriptor Descriptor { get; set; }

        public IEnumerable<Property> Properties
        {
            get { return properties; }
        }

        public IEnumerable<Model> Items
        {
            get { return items; }
        }

        #region Implementation of IPagedCollection

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }

        #endregion
    }
}