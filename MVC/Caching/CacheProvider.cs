using System;
using System.Web;
using System.Web.Caching;

namespace RunningObjects.MVC.Caching
{
    public class CacheProvider
    {
        #region Current Cache Provider
        private static CacheProvider provider;

        public static CacheProvider Current
        {
            get { return provider ?? (provider = new CacheProvider()); }
            set { provider = value; }
        } 
        #endregion

        public virtual object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public virtual object Add(string key, object entry, int duration)
        {
            return HttpRuntime.Cache.Add(key, entry, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(duration), CacheItemPriority.Normal, null);
        }

        public virtual object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }

        public virtual bool Contains(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }
    }
}
