using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace RunningObjects.Core.Query
{
    public class Select
    {
        public const char Separator = ',';
        private List<PropertyDescriptor> properties;
        
        public List<PropertyDescriptor> Properties
        {
            get { return properties ?? (properties = new List<PropertyDescriptor>()); }
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(CultureInfo.InvariantCulture), Properties.Select(p => p.Name).ToArray());
        }
    }
}