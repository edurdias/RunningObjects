using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace RunningObjects.MVC.Html
{
    public static class TextAreaExtensions
    {
        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;
        private static readonly Dictionary<string, object> ImplicitRowsAndColumns = new Dictionary<string, object> 
        {
            { "rows", TextAreaRows.ToString(CultureInfo.InvariantCulture) }, 
            { "cols", TextAreaColumns.ToString(CultureInfo.InvariantCulture) }, 
        };

        private static ModelMetadata GetMetadataFor(Parameter parameter)
        {
            return ModelMetadataProviders.Current.GetMetadataForType(() => parameter, typeof(Parameter));
        }


        private static Dictionary<string, object> GetRowsAndColumnsDictionary(int rows, int columns)
        {
            if (rows < 0)
                throw new ArgumentOutOfRangeException("rows");
            if (columns < 0)
                throw new ArgumentOutOfRangeException("columns");

            var result = new Dictionary<string, object>();
            if (rows > 0)
            {
                result.Add("rows", rows.ToString(CultureInfo.InvariantCulture));
            }
            if (columns > 0)
            {
                result.Add("cols", columns.ToString(CultureInfo.InvariantCulture));
            }

            return result;
        }

        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Parameter parameter)
        {
            return TextAreaFor(htmlHelper, parameter, null);
        }


        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Parameter parameter, object htmlAttributes)
        {
            return TextAreaFor(htmlHelper, parameter, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Parameter parameter, IDictionary<string, object> htmlAttributes)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return TextAreaHelper(htmlHelper,
                                  GetMetadataFor(parameter),
                                  parameter.Name,
                                  ImplicitRowsAndColumns,
                                  htmlAttributes);
        }


        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Parameter parameter, int rows, int columns, object htmlAttributes)
        {
            return TextAreaFor(htmlHelper, parameter, rows, columns, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Parameter parameter, int rows, int columns, IDictionary<string, object> htmlAttributes)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return TextAreaHelper(htmlHelper,
                                  GetMetadataFor(parameter),
                                  parameter.Name,
                                  GetRowsAndColumnsDictionary(rows, columns),
                                  htmlAttributes);
        }

        private static MvcHtmlString TextAreaHelper(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string name, IDictionary<string, object> rowsAndColumns, IDictionary<string, object> htmlAttributes)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("name");

            var tagBuilder = new TagBuilder("textarea");
            tagBuilder.GenerateId(fullName);
            tagBuilder.MergeAttributes(htmlAttributes, true);
            tagBuilder.MergeAttributes(rowsAndColumns, rowsAndColumns != ImplicitRowsAndColumns);  // Only force explicit rows/cols
            tagBuilder.MergeAttribute("name", fullName, true);

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState) && modelState.Errors.Count > 0)
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);

            tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name));

            string value;
            if (modelState != null && modelState.Value != null)
                value = modelState.Value.AttemptedValue;
            else if (modelMetadata.Model != null)
                value = modelMetadata.Model.ToString();
            else
                value = String.Empty;
            tagBuilder.SetInnerText(Environment.NewLine + value);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}