using System.ComponentModel;
using System.Web.Mvc;

namespace RunningObjects.Core
{
    public static class ModelExtensions
    {
        public static Parameter AsModel(this ParameterDescriptor descriptor)
        {
            return new Parameter(descriptor);
        }

        public static Property AsModel(this PropertyDescriptor descriptor)
        {
            return new Property(descriptor);
        }

        public static Property AsModel(this PropertyDescriptor descriptor, Model model)
        {
            return new Property(descriptor, model);
        }

    }
}