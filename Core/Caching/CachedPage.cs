using System;
using System.Web.UI;

namespace RunningObjects.Core.Caching
{
    internal sealed class CachedPage : Page
    {
        public CachedPage(OutputCacheParameters cacheSettings)
        {
            ID = Guid.NewGuid().ToString();
            CacheSettings = cacheSettings;
        }

	    private OutputCacheParameters CacheSettings { get; set; }

        protected override void FrameworkInitialize()
        {
            base.FrameworkInitialize();
            InitOutputCache(CacheSettings);
        }
    }
}