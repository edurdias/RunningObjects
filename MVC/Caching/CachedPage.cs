using System;
using System.Web.UI;

namespace RunningObjects.MVC.Caching
{
    internal class CachedPage : Page
    {
        public CachedPage(OutputCacheParameters cacheSettings)
        {
            ID = Guid.NewGuid().ToString();
            CacheSettings = cacheSettings;
        }

        protected OutputCacheParameters CacheSettings { get; set; }

        protected override void FrameworkInitialize()
        {
            base.FrameworkInitialize();
            InitOutputCache(CacheSettings);
        }
    }
}