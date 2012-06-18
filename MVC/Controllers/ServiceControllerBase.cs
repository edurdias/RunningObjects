using System;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC.Controllers
{
    public class ServiceControllerBase : ControllerBase
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
        public virtual ActionResult Create(Type modelType, Method method)
        {
            return CreateInstanceOf(modelType, method, Json, Throw);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult View(Type modelType, object key)
        {
            var mapping = ModelMappingManager.FindByType(modelType);
            var descriptor = new ModelDescriptor(mapping);
            var instance = GetInstanceOf(modelType, key, descriptor);
            if (instance == null)
                return HttpNotFound();
            return Json(instance, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public virtual ActionResult Edit(Type modelType, Model model)
        {
            return EditInstanceOf(modelType, model, Json, Throw, HttpNotFound);
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public virtual ActionResult Delete(Type modelType, object key)
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

        protected ActionResult ParseException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected static ActionResult Throw(Exception ex)
        {
            throw ex;
        }
    }
}