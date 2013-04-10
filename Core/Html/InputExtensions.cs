using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace RunningObjects.Core.Html
{
    public static class InputExtensions
    {

        private static ModelMetadata GetMetadataFor(Member member)
        {
            return ModelMetadataProviders.Current.GetMetadataForType(() => member, member.GetType());
        }


        // CheckBox

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            return CheckBoxFor(htmlHelper, member, null);
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object htmlAttributes)
        {
            return CheckBoxFor(htmlHelper, member, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, IDictionary<string, object> htmlAttributes)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var metadata = GetMetadataFor(member);
            bool? isChecked = null;
            if (metadata.Model != null)
            {
                bool modelChecked;
                if (Boolean.TryParse(metadata.Model.ToString(), out modelChecked))
                    isChecked = modelChecked;
            }
            var attributes = ToRouteValueDictionary(htmlAttributes);

            var explicitValue = isChecked.HasValue;
            if (explicitValue)
                attributes.Remove("checked");

            return InputHelper(htmlHelper, InputType.CheckBox, metadata, member.Name, "true", !explicitValue /* useViewData */, isChecked ?? false, true /* setId */, false /* isExplicitValue */, attributes);
        }

        // Hidden

        public static MvcHtmlString HiddenFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            return HiddenFor(htmlHelper, member, null);
        }


        public static MvcHtmlString HiddenFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object htmlAttributes)
        {
            return HiddenFor(htmlHelper, member, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString HiddenFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, IDictionary<string, object> htmlAttributes)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var metadata = GetMetadataFor(member);
            var value = metadata.Model;
            var binaryValue = value as Binary;
            if (binaryValue != null)
            {
                value = binaryValue.ToArray();
            }

            var byteArrayValue = value as byte[];
            if (byteArrayValue != null)
            {
                value = Convert.ToBase64String(byteArrayValue);
            }

            return InputHelper(htmlHelper, InputType.Hidden, metadata, member.Name, value, false, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes);
        }

        // Password 
        public static MvcHtmlString PasswordFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            return PasswordFor(htmlHelper, member, null /* htmlAttributes */);
        }


        public static MvcHtmlString PasswordFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object htmlAttributes)
        {
            return PasswordFor(htmlHelper, member, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString PasswordFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, IDictionary<string, object> htmlAttributes)
        {
            if (member == null)
                throw new ArgumentNullException("member");
            return InputHelper(htmlHelper, InputType.Password, GetMetadataFor(member), member.Name, null, false /* useViewData */, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes);
        }

        // RadioButton

        public static MvcHtmlString RadioButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object value)
        {
            return RadioButtonFor(htmlHelper, member, value, null /* htmlAttributes */);
        }


        public static MvcHtmlString RadioButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object value, object htmlAttributes)
        {
            return RadioButtonFor(htmlHelper, member, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString RadioButtonFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object value, IDictionary<string, object> htmlAttributes)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var metadata = GetMetadataFor(member);
            var attributes = ToRouteValueDictionary(htmlAttributes);
            var valueString = Convert.ToString(value, CultureInfo.CurrentCulture);
            var isChecked = metadata.Model != null &&
                              !String.IsNullOrEmpty(member.Name) &&
                              String.Equals(metadata.Model.ToString(), valueString, StringComparison.OrdinalIgnoreCase);

            return InputHelper(htmlHelper, InputType.Radio, metadata, member.Name, value, false /* useViewData */, isChecked, true /* setId */, true /* isExplicitValue */, attributes);
        }

        // TextBox 

        public static MvcHtmlString TextBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            return htmlHelper.TextBoxFor(member, null);
        }


        public static MvcHtmlString TextBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, object htmlAttributes)
        {
            return htmlHelper.TextBoxFor(member, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString TextBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member, IDictionary<string, object> htmlAttributes)
        {
            var metadata = GetMetadataFor(member);
            return InputHelper(htmlHelper, InputType.Text, metadata, member.Name, metadata.Model, false /* useViewData */, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes);
        }

        // Helper methods

        private static MvcHtmlString InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("name");

            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
            tagBuilder.MergeAttribute("name", fullName, true);

            var valuemember = Convert.ToString(value, CultureInfo.CurrentCulture);
            var usedModelState = false;

            switch (inputType)
            {
                case InputType.CheckBox:
                    var modelStateWasChecked = GetModelStateValue(htmlHelper, fullName, typeof(bool)) as bool?;
                    if (modelStateWasChecked.HasValue)
                    {
                        isChecked = modelStateWasChecked.Value;
                        usedModelState = true;
                    }
                    goto case InputType.Radio;
                case InputType.Radio:
                    if (!usedModelState)
                    {
                        var modelStateValue = GetModelStateValue(htmlHelper, fullName, typeof(string)) as string;
                        if (modelStateValue != null)
                        {
                            isChecked = String.Equals(modelStateValue, valuemember, StringComparison.Ordinal);
                            usedModelState = true;
                        }
                    }
                    if (!usedModelState && useViewData)
                    {
                        isChecked = Convert.ToBoolean(htmlHelper.ViewData.Eval(fullName), CultureInfo.InvariantCulture);
                    }
                    if (isChecked)
                    {
                        tagBuilder.MergeAttribute("checked", "checked");
                    }
                    tagBuilder.MergeAttribute("value", valuemember, isExplicitValue);
                    break;
                case InputType.Password:
                    if (value != null)
                    {
                        tagBuilder.MergeAttribute("value", valuemember, isExplicitValue);
                    }
                    break;
                default:
                    var attemptedValue = (string)GetModelStateValue(htmlHelper, fullName, typeof(string));
                    tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? EvalString(htmlHelper, fullName) : valuemember), isExplicitValue);
                    break;
            }

            if (setId)
            {
                tagBuilder.GenerateId(fullName);
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

            if (inputType == InputType.CheckBox)
            {
                // Render an additional <input type="hidden".../> for checkboxes. This 
                // addresses scenarios where unchecked checkboxes are not sent in the request.
                // Sending a hidden input makes it possible to know that the checkbox was present 
                // on the page when the request was submitted. 
                var inputItemBuilder = new StringBuilder();
                inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

                var hiddenInput = new TagBuilder("input");
                hiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
                hiddenInput.MergeAttribute("name", fullName);
                hiddenInput.MergeAttribute("value", "false");
                inputItemBuilder.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
                return MvcHtmlString.Create(inputItemBuilder.ToString());
            }

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static RouteValueDictionary ToRouteValueDictionary(IDictionary<string, object> dictionary)
        {
            return dictionary == null ? new RouteValueDictionary() : new RouteValueDictionary(dictionary);
        }

        private static string EvalString(HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
        }

        private static object GetModelStateValue(HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }
    }
}