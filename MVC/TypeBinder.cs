using System.Web.Mvc;
using System.Collections.Generic;

namespace RunningObjects.MVC
{
    public class TypeBinder : IModelBinder
    {
        internal const string ModeTypeKey = "modelType";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(ModeTypeKey);
            if (result != null)
            {
                var partialTypeName = result.AttemptedValue;
                foreach (var pair in ModelAssemblies.Assemblies)
                {
                    var typeName = ConvertTypeName(string.Format("{0}.{1}", pair.Key, partialTypeName));
                    foreach (var asmType in pair.Value.GetTypes())
                    {
                        var asmTypeName = ConvertTypeName(asmType.FullName);
                        if (typeName == asmTypeName)
                            return asmType;
                    }
                }
                throw new RunningObjectsException(string.Format("Model type '{0}' cannot be found. Please check the correct type name.", partialTypeName));
            }
            return null;
        }

        private static string ConvertTypeName(string typeName)
        {
            return typeName
                .Replace("_", string.Empty)
                .ToLowerInvariant();
        }
    }
}