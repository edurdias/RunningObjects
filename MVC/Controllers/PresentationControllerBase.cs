using System;
using System.Linq;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC.Controllers
{
    public class PresentationControllerBase : ControllerBase
    {
        public virtual ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Index(Type modelType, int page = 1, int? size = null)
        {
            ViewBag.Exceptions = Exceptions;

            var query = GetQueryById(modelType) ?? GetDefaultQueryOf(modelType);
            return CacheableView("Index", query, () =>
            {
                var items = query.Execute(true);
                var quantity = size.HasValue ? size.Value : query.Paging ? query.PageSize : items.Count();
                var fetched = items.Skip((page - 1) * quantity).Take(quantity);

                var mapping = ModelMappingManager.MappingFor(fetched.ElementType);
                var descriptor = new ModelDescriptor(mapping);

                return new ModelCollection(modelType, descriptor, fetched)
                {
                    PageCount = quantity > 0 ? (int)Math.Ceiling((decimal)items.Count() / quantity) : items.Count(),
                    PageNumber = page,
                    PageSize = quantity
                };
            },
            settings => settings.VaryByParam = "page;size");
        }

        public ActionResult Create(Type modelType, int index)
        {
            var typeMapping = ModelMappingManager.FindByType(modelType);
            var mapping = typeMapping.Constructors.FirstOrDefault(m => m.Index == index);
            if (mapping == null)
                throw new RunningObjectsException(string.Format("No constructor found at index {0} for type {1}", index, modelType.PartialName()));

            var method = new Method(new MethodDescriptor(mapping, ControllerContext.GetActionDescriptor(RunningObjectsAction.Create)));
            return !ControllerContext.IsChildAction ? (ActionResult) View(method) : PartialView(method);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Type modelType, Method model)
        {
            return CreateInstanceOf
            (
                modelType,
                model,
                OnSuccess(modelType),
                OnException(model)
            );
        }

        public ActionResult View(Type modelType, object key)
        {
            var mapping = ModelMappingManager.FindByType(modelType);
            var descriptor = new ModelDescriptor(mapping);
            var instance = GetInstanceOf(modelType, key, descriptor);
            if (instance == null)
                return HttpNotFound();
            var model = new Model(modelType, descriptor, instance);
            return !ControllerContext.IsChildAction ? (ActionResult) View(model) : PartialView(model);
        }

        public ActionResult Edit(Type modelType, object key)
        {
            var mapping = ModelMappingManager.FindByType(modelType);
            var descriptor = new ModelDescriptor(mapping);
            var instance = GetInstanceOf(modelType, key, descriptor);
            if (instance == null)
                return HttpNotFound();
            var model = new Model(modelType, descriptor, instance);
            return !ControllerContext.IsChildAction ? (ActionResult) View(model) : PartialView(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Type modelType, Model model)
        {
            return EditInstanceOf
            (
                modelType, //model type
                model, // model instance
                OnSuccess(modelType), //what to do in case of success
                OnException(model), //what to do in case of exception
                HttpNotFound //what to do in case of not found
            );
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Type modelType, object key)
        {
            return DeleteInstanceOf
            (
                modelType,
                key,
                () => OnSuccess(modelType)(null),
                ex => RedirectToAction("Index", new { modelType = modelType.PartialName() }),
                HttpNotFound
            );
        }

        public ActionResult Execute(Type modelType, string methodName, int index, string key = null)
        {
            var typeMapping = ModelMappingManager.FindByType(modelType);
            var methods = key != null ? typeMapping.InstanceMethods : typeMapping.StaticMethods;
            var mapping = methods.FirstOrDefault(m => m.MethodName.Equals(methodName, StringComparison.InvariantCultureIgnoreCase) && m.Index == index);
            if (mapping == null)
                throw new RunningObjectsException(string.Format("No method found with name {2} at index {0} for type {1}", index, modelType.PartialName(), methodName));

            var method = new Method(new MethodDescriptor(mapping, ControllerContext.GetActionDescriptor(RunningObjectsAction.Execute)));

            if (!method.Parameters.Any())
            {
                return ExecuteMethodOf
                (
                    modelType,
                    method,
                    () => OnSuccess(modelType)(null),
                    OnSuccessWithReturn(method),
                    OnException(method),
                    HttpNotFound,
                    key
                );
            }

            return !ControllerContext.IsChildAction ? (ActionResult) View(method) : PartialView(method);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Execute(Type modelType, Method model, string key = null)
        {
            return ExecuteMethodOf
            (
                modelType,
                model,
                () => OnSuccess(modelType)(null),
                OnSuccessWithReturn(model),
                OnException(model),
                HttpNotFound,
                key
            );
        }

        private Func<object, ActionResult> OnSuccess(Type modelType)
        {
            return instance =>
            {
                var redirectTo = ControllerContext.HttpContext.Request["redirectTo"];
                if (!string.IsNullOrEmpty(redirectTo))
                    return Redirect(redirectTo);
                return RedirectToAction("Index", new { modelType = modelType.PartialName() });
            };
        }

        private Func<Exception, ActionResult> OnException(object model)
        {
            return ex =>
            {
                ViewBag.Exceptions = Exceptions;
                return View(model);
            };
        }

        protected Func<object, ActionResult> OnSuccessWithReturn(Method model)
        {
            return @return =>
            {
                var result = ParseResult(model, @return);
                if (result is ModelCollection)
                    return View("Index", result);
                var redirectTo = ControllerContext.HttpContext.Request["redirectTo"];
                if (!string.IsNullOrEmpty(redirectTo))
                    return Redirect(redirectTo);
                return null;
            };
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(HttpNotFoundResult))
                filterContext.Result = View("404");
            base.OnActionExecuted(filterContext);
        }
    }
}