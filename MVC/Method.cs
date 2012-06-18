using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RunningObjects.MVC
{
    public sealed class Method
    {
        private List<Parameter> parameters;

        internal Method(MethodDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public MethodDescriptor Descriptor { get; private set; }

        public object Instance { get; set; }

        public IEnumerable<Parameter> Parameters
        {
            get
            {
                return parameters ?? (parameters = Descriptor.Parameters.Select(parameter => parameter.AsModel()).ToList());
            }
        }

        public object Invoke(object obj)
        {
            var args = Parameters.Select(p =>
            {
                if (p.Value is IConvertible)
                {
                    var parameterType = p.Descriptor.ParameterType;
                    if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof (Nullable<>))
                        parameterType = parameterType.GetGenericArguments()[0];
                    return Convert.ChangeType(p.Value, parameterType);
                }
                return p.Value;
            }).ToArray();

            return Descriptor.Method is ConstructorInfo
                       ? ((ConstructorInfo) Descriptor.Method).Invoke(args)
                       : Descriptor.Method.Invoke(obj, args);
        }
    }
}