using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RunningObjects.MVC
{
    [DefaultProperty("Value")]
    public class Property : Member
    {
        public Property(PropertyDescriptor descriptor, Model model = null)
            : base(descriptor.PropertyType)
        {
            if (descriptor == null)
                throw new ArgumentNullException("descriptor");

            Descriptor = descriptor;
            Model = model;
        }

        public PropertyDescriptor Descriptor { get; private set; }

        public override string Name { get { return Descriptor.Name; } }

        public override object Value
        {
            get
            {
                return Model != null ? Descriptor.GetValue(Model.Instance) : null;
            }
            set
            {
                if (Model != null)
                    Descriptor.SetValue(Model.Instance, value);
            }
        }

        public override IEnumerable<Attribute> Attributes
        {
            get { return Descriptor.Attributes.OfType<Attribute>(); }
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : UnderliningModel != null && UnderliningModel.TextValue != null ? UnderliningModel.TextValue.ToString() : string.Empty;
        }
    }
}