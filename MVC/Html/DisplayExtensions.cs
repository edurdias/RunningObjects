using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RunningObjects.MVC.Html
{
    public static class DisplayExtensions
    {
        public static MvcHtmlString DisplayFor<TModel>(this HtmlHelper<TModel> htmlHelper, Member member)
        {
            var names = GetTemplateNames(member);
            foreach (var result in names.Select(name => GetDisplayFor(htmlHelper, member, name)))
                if (result != null && !string.IsNullOrEmpty(result.ToHtmlString()))
                    return result;
            return new MvcHtmlString(htmlHelper.Encode(member.Value));
        }

        internal static IEnumerable<string> GetTemplateNames(Member member)
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

        private static MvcHtmlString GetDisplayFor<TModel>(HtmlHelper<TModel> htmlHelper, Member member, string templateName)
        {
            var result = htmlHelper.DisplayFor(m => member, templateName, member.Name);
            if (result != null && !string.IsNullOrEmpty(result.ToHtmlString()))
                return result;
            return result;
        }
    }
}