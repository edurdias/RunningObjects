using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.Core.Mapping;
using RunningObjects.Core.Query;

namespace RunningObjects.Core
{
    public static class QueryExtensions
    {
        public static Query.Query Query(this Member member, ControllerContext controllerContext)
        {
            var modelType = member.UnderliningModel.ModelType;
            var repository = modelType.Repository();
            var source = repository.All();
            var attr = member.Attributes.OfType<QueryAttribute>().FirstOrDefault();
            return attr != null
                ? QueryParser.Parse(modelType, source, attr)
                : QueryParser.Parse(modelType, source);
        }

        public static IEnumerable<SelectListItem> GetSelectListItems(this Member member, ControllerContext controllerContext, object selectedValue)
        {
            return GetSelectListItems(member, member.Query(controllerContext).Execute(), selectedValue);
        }

        public static IEnumerable<SelectListItem> GetSelectListItems(this Member member, IQueryable source, object selectedValue)
        {
            var items = new List<SelectListItem> {new SelectListItem()};
            if (member.IsModel)
            {
                var mapping = ModelMappingManager.MappingFor(source.ElementType);
                var elementDescriptor = new ModelDescriptor(mapping);
                foreach (object item in source)
                {
                    var listItem = new SelectListItem();
                    var value = elementDescriptor.KeyProperty.GetValue(item);
                    listItem.Value = value != null ? value.ToString() : item.ToString();

                    if (elementDescriptor.TextProperty != null)
                    {
                        var textProperty = elementDescriptor.TextProperty.GetValue(item);
                        if (textProperty != null)
                            listItem.Text = textProperty.ToString();
                    }
                    else
                    {
                        var boxed = item;
                        listItem.Text = boxed == null ? string.Empty : boxed.ToString();
                    }

                    if (selectedValue != null)
                    {
                        var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(member.MemberType));
                        var modelValue = descriptor.KeyProperty.GetValue(selectedValue);
                        if (modelValue != null)
                            listItem.Selected = listItem.Value == modelValue.ToString();
                    }

                    items.Add(listItem);
                }
            }
            return items;
        }
    }
}