using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace RunningObjects.Core.Html
{
    public static class LabelExtensions
    {
        #region Property Extensions
        public static MvcHtmlString LabelFor(this HtmlHelper html, Property property)
        {
            return LabelFor(html, property, null, null);
        }

        public static MvcHtmlString LabelFor(this HtmlHelper html, Property property, object htmlAttributes)
        {
            return LabelFor(html, property, null, htmlAttributes);
        }

        public static MvcHtmlString LabelFor(this HtmlHelper html, PropertyDescriptor descriptor)
        {
            return LabelFor(html, descriptor.AsModel(), null, null);
        }

        public static MvcHtmlString TextFor(this HtmlHelper html, PropertyDescriptor descriptor)
        {
            var property = descriptor.AsModel();
            return new MvcHtmlString(ResolveText(property, GetMetadata(property)));
        }
        #endregion

        #region Parameter Extensions
        public static MvcHtmlString LabelFor(this HtmlHelper html, Parameter parameter)
        {
            return LabelFor(html, parameter.Descriptor.AsModel(), null, null);
        }

        public static MvcHtmlString LabelFor(this HtmlHelper html, Parameter parameter, object htmlAttributes)
        {
            return LabelFor(html, parameter.Descriptor.AsModel(), null, htmlAttributes);
        } 
        #endregion

        #region Member Extensions
        public static MvcHtmlString TextFor(this HtmlHelper html, Member property)
        {
            return new MvcHtmlString(ResolveText(property, GetMetadata(property)));
        }

        public static MvcHtmlString LabelFor(this HtmlHelper html, Member member, string labelText, object htmlAttributes)
        {
            var metadata = GetMetadata(member);
            var resolvedLabelText = ResolveText(member, metadata, labelText);
            if (String.IsNullOrEmpty(resolvedLabelText))
                return MvcHtmlString.Empty;

            var tag = new TagBuilder("label");
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(member.Name)));
            tag.SetInnerText(resolvedLabelText);
            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        } 
        #endregion

        #region Utilities
        private static string ResolveText(Member member, ModelMetadata metadata, string defaultText = null)
        {
            return !string.IsNullOrEmpty(defaultText)
                       ? defaultText
                       : !string.IsNullOrEmpty(metadata.DisplayName)
                             ? metadata.DisplayName
                             : !string.IsNullOrEmpty(metadata.PropertyName)
                                   ? metadata.PropertyName
                                   : member.Name.Split('.').Last();
        }

        private static ModelMetadata GetMetadata(Member member)
        {
            return ModelMetadataProviders.Current.GetMetadataForProperty(() => member, member.GetType(), member.Name);
        } 
        #endregion
    }
}