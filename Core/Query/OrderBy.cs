using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RunningObjects.Core.Query
{
    public class OrderBy
    {
        private Dictionary<string, string> elements;
        public const char Separator = ':';
        public const char ElementSeparator = ',';

        public Dictionary<string, string> Elements
        {
            get { return elements ?? (elements = new Dictionary<string, string>()); }
        }

        public static bool IsValid(string orderBy)
        {
            return Regex.IsMatch(orderBy, @"([a-z0-9_]+):(Asc|Desc)");
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var element in Elements)
                builder.Append(element.Key + Separator + element.Value);
            return builder.ToString();
        }
    }
}