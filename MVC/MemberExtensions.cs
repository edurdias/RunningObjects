using System.Collections;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC
{
    public static class MemberExtensions
    {
        public static ModelCollection AsCollection(this Member member, ControllerContext controllerContext)
        {
            if (member.IsModelCollection)
            {
                var model = member.UnderliningModel;
                
                var items = default(IQueryable);
                if (member.Value != null)
                    items = ((IEnumerable)member.Value).AsQueryable();
                else if (member is Parameter)
                {
                    var context = ModelAssemblies.GetContext(model.ModelType);
                    items = context.Set(model.ModelType);
                }
                
                var attr = member.Attributes.OfType<QueryAttribute>().FirstOrDefault();
                var result = QueryParser.Parse(model.ModelType, items, attr).Execute(true);
                var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(result.ElementType));
                return new ModelCollection(model.ModelType, descriptor, result);
            }
            return null;
        }
    }
}