using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RunningObjects.MVC.Html
{
    public static class EditorExtensions
    {
        public static MvcHtmlString EditorFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            foreach (var name in GetTemplateNames(member))
            {
                var result = GetEditorFor(htmlHelper, member, name);
                if (result != null && !string.IsNullOrEmpty(result.ToHtmlString()))
                    return result;
            }

            return new MvcHtmlString(htmlHelper.Encode(member.Value));
        }

        private static IEnumerable<string> GetTemplateNames(Member member)
        {
            var names = new List<string>();

            var specificName = member is Parameter
                ? "Parameter" 
                : member is Property 
                    ? "Property" 
                    : "Member";

            if (member.IsModelCollection)
            {
                names.Add(specificName + "Collection");
                names.Add("MemberCollection");
            }
            else if (member.IsModel)
            {
                names.Add(specificName);
                names.Add("Member");
            }

            names.Add(member.Name);
            names.Add(member.MemberType.Name);
            names.Add(null);
            return names;
        }

        private static MvcHtmlString GetEditorFor<TModel>(HtmlHelper<TModel> htmlHelper, Member member, string templateName)
        {
            var result = htmlHelper.EditorFor(m => member, templateName, member.Name);
            if (result != null && !string.IsNullOrEmpty(result.ToHtmlString()))
                return result;
            return result;
        }
    }
}