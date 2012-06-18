using System;

namespace RunningObjects.MVC.Query
{
    public class DynamicProperty
    {
        string name;
        Type type;
        Attribute[] attributes;

        public DynamicProperty(string name, Type type, Attribute[] attributes)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");
            this.name = name;
            this.type = type;
            this.attributes = attributes;
        }

        public string Name
        {
            get { return name; }
        }

        public Type Type
        {
            get { return type; }
        }

        public Attribute[] Attributes
        {
            get { return attributes; }
        }
    }
}