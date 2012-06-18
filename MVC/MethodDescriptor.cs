using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC
{
    public sealed class MethodDescriptor : MemberDescriptor
    {
        private readonly List<ParameterDescriptor> parameters = new List<ParameterDescriptor>();

        public MethodDescriptor(MethodMapping mapping, ActionDescriptor actionDescriptor)
            :base(mapping.Name)
        {
            Method = mapping.Method;
            Index = mapping.Index;
            AttributeArray = Method.GetCustomAttributes(true).OfType<Attribute>().ToArray();
            parameters.AddRange(Method.GetParameters().Select(info => new ReflectedParameterDescriptor(info, actionDescriptor)));
        }

        public MethodBase Method { get; private set; }

        public int Index { get; private set; }

        public IEnumerable<ParameterDescriptor> Parameters
        {
            get { return parameters; }
        }
    }
}