using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RunningObjects.MVC
{
    public class RunningObjectsValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<System.Attribute> attributes)
        {
            if(metadata.Model is Member)
            {
                var model = metadata.Model as Member;
                attributes = attributes.Concat(model.Attributes);
            }
            return base.GetValidators(metadata, context, attributes);
        }
    }
}