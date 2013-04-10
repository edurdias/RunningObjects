using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RunningObjects.Core.Mapping;
using RunningObjects.Core.Query;

namespace RunningObjects.Core.Controllers
{
    public sealed class ServiceController : ControllerBase
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(Type modelType, int page = 1, int? size = null)
        {
            var items = GetDefaultQueryOf(modelType).Execute();
            var quantity = size.HasValue ? size.Value : items.Count();
            var fetched = items.Skip((page - 1) * quantity).Take(quantity);
            return Json(fetched, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Type modelType, Method method)
        {
            return CreateInstanceOf(modelType, method, Json, Throw);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(Type modelType, object key)
        {
            var mapping = ModelMappingManager.MappingFor(modelType);
            var descriptor = new ModelDescriptor(mapping);
            var instance = GetInstanceOf(modelType, key, descriptor);
            if (instance == null)
                return HttpNotFound();
            return Json(instance, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Edit(Type modelType, Model model)
        {
            return EditInstanceOf(modelType, model, Json, Throw, HttpNotFound);
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(Type modelType, object key)
        {
            return DeleteInstanceOf(modelType, key, () => Json(true), Throw, HttpNotFound);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Execute(Type modelType, Method model, string key = null)
        {
            return ExecuteMethodOf(modelType, model, () => Json(true), Json, Throw, HttpNotFound, key);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = ParseException(filterContext.Exception);
                filterContext.ExceptionHandled = true;
            }
            base.OnException(filterContext);
        }

	    private ActionResult ParseException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        }

	    private static ActionResult Throw(Exception ex)
        {
            throw ex;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new EnhancedJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        private class EnhancedJsonResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;

                response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;
                if (Data != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ContractResolver = new ContractResolver()
                    };

                    response.Write(JsonConvert.SerializeObject(Data, Formatting.None, settings));
                }
            }

            private class ContractResolver : DefaultContractResolver
            {
                protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);
                    property.ShouldSerialize = o => !member.GetCustomAttributes(true).OfType<ScriptIgnoreAttribute>().Any();
                    return property;
                }
            }
        }

    }

    
}