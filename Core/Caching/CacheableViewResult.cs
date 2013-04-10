using System;
using System.Web.Mvc;
using System.Web.UI;

namespace RunningObjects.Core.Caching
{
    public class CacheableViewResult : ViewResult
    {
        public OutputCacheParameters CacheSettings { get; set; }

        public Func<object> ModelAccessor { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            ViewData.Model = ModelAccessor();
            base.ExecuteResult(context);
        }
    }
}