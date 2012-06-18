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

        private static IEnumerable<string> GetTemplateNames(Member member)
        {
            var names = new List<string>();
            if (member.IsModel)
                names.Add("Member");
            else if (member.IsModelCollection)
                names.Add("MemberCollection");
            else
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