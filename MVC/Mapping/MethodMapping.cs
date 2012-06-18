using System;
using System.Collections.Generic;
using System.Reflection;

namespace RunningObjects.MVC.Mapping
{
    public class MethodMapping : IElementMapping
    {
        private IEnumerable<ParameterInfo> parameters;

        public string ID { get; set; }

        public string Name { get; internal set; }

        public string MethodName { get; internal set; }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return parameters ?? (parameters = new List<ParameterInfo>()); }
            internal set { parameters = value; }
        }

        public int Index { get; internal set; }

        public TypeMapping Type { get; set; }

        public Type ReturnType { get; internal set; }

        public RunningObjectsAction UnderlineAction { get; internal set; }

        public IElementMapping Parent
        {
            get { return Type; }
        }

        public bool Visible { get; set; }

        public MethodBase Method { get; set; }
    }
}