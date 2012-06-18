using System;
using System.ComponentModel.DataAnnotations;

namespace RunningObjects.MVC
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public class ParameterUIHintAttribute : UIHintAttribute
    {
        public ParameterUIHintAttribute(string uiHint) : base(uiHint)
        {
        }

        public ParameterUIHintAttribute(string uiHint, string presentationLayer) : base(uiHint, presentationLayer)
        {
        }

        public ParameterUIHintAttribute(string uiHint, string presentationLayer, params object[] controlParameters) : base(uiHint, presentationLayer, controlParameters)
        {
        }
    }
}