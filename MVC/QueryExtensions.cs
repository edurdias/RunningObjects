using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC
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
            var items = new List<SelectListItem> { new SelectListItem() };
            if(member.IsModel)
            {
                var result = member.Query(controllerContext).Execute();

                var mapping = ModelMappingManager.MappingFor(result.ElementType);
                var elementDescriptor = new ModelDescriptor(mapping);
                foreach (object item in result)
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