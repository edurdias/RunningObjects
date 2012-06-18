using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RunningObjects.MVC.Html
{
    public static class SelectExtensions
    {
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, Member member, IEnumerable<SelectListItem> selectList)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(() => member, member.GetType());
            return SelectInternal(htmlHelper, null, "", selectList, metadata, false, null);
        }

        private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
        {
            object o = null;
            if (htmlHelper.ViewData != null)
                o = htmlHelper.ViewData.Eval(name);
            if (o == null)
                throw new InvalidOperationException();

            var selectList = o as IEnumerable<SelectListItem>;
            if (selectList == null)
                throw new InvalidOperationException();

            return selectList;
        }

        public static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
                if (modelState.Value != null)
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
            return null;
        }

        internal static string ListItemToOption(SelectListItem item)
        {
            var builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            return builder.ToString(TagRenderMode.Normal);
        }


        private static MvcHtmlString SelectInternal(HtmlHelper htmlHelper, string optionLabel, string name, IEnumerable<SelectListItem> selectList, ModelMetadata metadata, bool allowMultiple, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("name");

            var usedViewData = false;

            // If we got a null selectList, try to use ViewData to get the list of items. 
            if (selectList == null)
            {
                selectList = htmlHelper.GetSelectData(fullName);
                usedViewData = true;
            }

            object defaultValue = (allowMultiple) ? htmlHelper.GetModelStateValue(fullName, typeof(string[])) : htmlHelper.GetModelStateValue(fullName, typeof(string));

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value. 
            if (!usedViewData)
            {
                if (defaultValue == null)
                {
                    defaultValue = htmlHelper.ViewData.Eval(fullName);
                }
            }

            if (defaultValue != null)
            {
                var defaultValues = (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue };
                var values = from object value in defaultValues select Convert.ToString(value, CultureInfo.CurrentCulture);
                var selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                var newSelectList = new List<SelectListItem>();

                foreach (var item in selectList)
                {
                    item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);
                }
                selectList = newSelectList;
            }

            // Convert each ListItem to an <option> tag
            var listItemBuilder = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
            {
                listItemBuilder.AppendLine(ListItemToOption(new SelectListItem { Text = optionLabel, Value = String.Empty, Selected = false }));
            }

            foreach (var item in selectList)
            {
                listItemBuilder.AppendLine(ListItemToOption(item));
            }

            var tagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId(fullName);
            if (allowMultiple)
            {
                tagBuilder.MergeAttribute("multiple", "multiple");
            }

            // If there are any errors for a named field, we add the css attribute. 
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
