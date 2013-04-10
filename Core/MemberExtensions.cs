using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RunningObjects.Core.Mapping;
using RunningObjects.Core.Query;

namespace RunningObjects.Core
{
    public static class MemberExtensions
    {
        public static ModelCollection ToModelCollection(this Member member)
        {
            if (member.IsModelCollection)
            {
                var model = member.UnderliningModel;

                var items = (member.Value != null)
                                ? ((IEnumerable)member.Value)
                                : ((IEnumerable)Activator.CreateInstance(typeof(List<>).MakeGenericType(member.UnderliningModel.ModelType)));

                var attr = member.Attributes.OfType<QueryAttribute>().FirstOrDefault();
                var result = QueryParser.Parse(model.ModelType, items.AsQueryable(), attr).Execute(true);

                var type = result != null ? result.ElementType : model.ModelType;
                var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(type));
                return new ModelCollection(type, descriptor, result);

            }
            return null;
        }
    }
}