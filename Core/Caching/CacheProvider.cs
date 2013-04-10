using System;
using System.Web;
using System.Web.Caching;

namespace RunningObjects.Core.Caching
{
    public sealed class CacheProvider
    {
        #region Current Cache Provider
        private static CacheProvider _provider;

        public static CacheProvider Current
        {
            get { return _provider ?? (_provider = new CacheProvider()); }
            set { _provider = value; }
        } 
        #endregion

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public static object Add(string key, object entry, int duration)
        {
	        return HttpRuntime.Cache.Add(key, entry, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(duration), CacheItemPriority.Normal, null);
        }

		public static object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }
    }
}
