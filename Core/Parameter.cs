using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RunningObjects.Core
{
    public class Parameter : Member
    {
        private object value;

        public Parameter(ParameterDescriptor descriptor)
            : base(descriptor.ParameterType)
        {
            Descriptor = descriptor;
        }

        public ParameterDescriptor Descriptor { get; private set; }


        public override string Name
        {
            get { return Descriptor.ParameterName; }
        }



        public override object Value
        {
            get { return value ?? Descriptor.DefaultValue; }
            set { this.value = value; }
        }

        public override IEnumerable<Attribute> Attributes
        {
            get { return Descriptor.GetCustomAttributes(false).OfType<Attribute>(); }
        }



        public override string ToString()
        {
            return (string)Value;
        }
    }
}