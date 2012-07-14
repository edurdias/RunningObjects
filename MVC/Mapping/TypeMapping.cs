using System;
using System.Collections.Generic;
using System.ComponentModel;
using RunningObjects.MVC.Mapping.Configuration;

namespace RunningObjects.MVC.Mapping
{
    public class TypeMapping : IMappingElement
    {
        private IEnumerable<MethodMapping> constructors;
        private IEnumerable<MethodMapping> staticMethods;
        private IEnumerable<MethodMapping> instanceMethods;
        private IEnumerable<QueryMapping> queries;
        private IEnumerable<PropertyDescriptor> properties;

        public string ID { get; set; }

        public string Name { get; internal set; }

        public Type ModelType { get; internal set; }

        public NamespaceMapping Namespace { get; set; }

        public IEnumerable<PropertyDescriptor> Properties
        {
            get { return properties ?? (properties = new List<PropertyDescriptor>()); }
            set { properties = value; }
        }

        public PropertyDescriptor Key { get; internal set; }

        public PropertyDescriptor Text { get; internal set; }


        public IEnumerable<MethodMapping> Constructors
        {
            get { return constructors ?? (constructors = new List<MethodMapping>()); }
            internal set { constructors = value; }
        }

        public IEnumerable<MethodMapping> StaticMethods
        {
            get { return staticMethods ?? (staticMethods = new List<MethodMapping>()); }
            internal set { staticMethods = value; }
        }

        public IEnumerable<MethodMapping> InstanceMethods
        {
            get { return instanceMethods ?? (instanceMethods = new List<MethodMapping>()); }
            internal set { instanceMethods = value; }
        }

        public IEnumerable<QueryMapping> Queries
        {
            //TODO:Create resource for string value
            get { return queries ?? (queries = new List<QueryMapping> { new QueryMapping { Name = "All Items", Type = this } }); }
            set { queries = value; }
        }

        public IMappingElement Parent
        {
            get { return !Namespace.IsRoot ? Namespace : null; }
        }

        public bool Visible { get; set; }

        public TypeMappingConfiguration Configuration { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TypeMapping)) return false;
            return Equals((TypeMapping)obj);
        }

        public bool Equals(TypeMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && other.ModelType == ModelType && Equals(other.Key, Key) && Equals(other.Text, Text);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (ModelType != null ? ModelType.GetHashCode() : 0);
                result = (result * 397) ^ (Key != null ? Key.GetHashCode() : 0);
                result = (result * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                return result;
            }
        }
    }
}