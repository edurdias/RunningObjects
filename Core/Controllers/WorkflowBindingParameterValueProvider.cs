using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace RunningObjects.Core.Controllers
{
	public class WorkflowBindingParameterValueProvider : IValueProvider
	{
		internal static IDictionary<string, object> BindingParameters
		{
			get
			{
				if (HttpContext.Current.Session["BindingParameters"] == null)
					HttpContext.Current.Session["BindingParameters"] = new Dictionary<string, object>();
				return (IDictionary<string, object>)HttpContext.Current.Session["BindingParameters"];
			}
		}

		public bool ContainsPrefix(string prefix)
		{
			return CollectionContainsPrefix(BindingParameters.Keys, prefix);
		}

		public ValueProviderResult GetValue(string key)
		{
			if(string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");
			var rawValue = BindingParameters.ContainsKey(key) ? BindingParameters[key] : null;
			return rawValue == null ? null : new ValueProviderResult(rawValue, Convert.ToString(rawValue), CultureInfo.CurrentCulture);
		}

        
		public static bool CollectionContainsPrefix(IEnumerable<string> collection, string prefix)
		{
			foreach (var key in collection)
			{
				if (key != null)
				{
					if (prefix.Length == 0)
						return true;

					if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
					{
						if (key.Length == prefix.Length)
							return true;

						switch (key[prefix.Length])
						{
							case '.':
							case '[':
								return true;
						}
					}
				}
			}
			return false;
		}
	}
}