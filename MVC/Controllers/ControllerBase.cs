using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using RunningObjects.MVC.Caching;
using RunningObjects.MVC.Logging;
using RunningObjects.MVC.Mapping;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC.Controllers
{
    public class ControllerBase : Controller
    {
        #region Caching

        protected ActionResult CacheableView(string viewName, Query.Query query, Func<ModelCollection> modelAccessor, Action<OutputCacheParameters> changeSettings)
        {
            if (query.IsCacheable)
            {
                var settings = new OutputCacheParameters
                {
                    Duration = query.CacheDuration,
                    VaryByParam = null,
                    VaryByContentEncoding = null,
                    VaryByControl = null,
                    VaryByCustom = null,
                    VaryByHeader = null
                };

                changeSettings(settings);

                return new CacheableViewResult
                {
                    ViewName = viewName,
                    ViewData = ViewData,
                    TempData = TempData,
                    ModelAccessor = modelAccessor,
                    CacheSettings = settings
                };
            }
            return View(viewName, modelAccessor());
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is CacheableViewResult && !filterContext.IsChildAction)
            {
                var view = filterContext.Result as CacheableViewResult;
                using (var page = new CachedPage(view.CacheSettings))
                    page.ProcessRequest(System.Web.HttpContext.Current);
            }
            else
                base.OnResultExecuting(filterContext);
        }
        #endregion

        #region Exception Handling

        protected List<Exception> Exceptions
        {
            get
            {
                if (!TempData.ContainsKey("exceptions"))
                    TempData.Add("exceptions", new List<Exception>());
                return TempData["exceptions"] as List<Exception>;
            }
        }

        protected void HandleException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            LoggingProviders.Current.OnException(new ExceptionContext(ControllerContext, ex));

            if (ex is ArgumentException)
            {
                var argex = ex as ArgumentException;
                ModelState.AddModelError(argex.ParamName, argex.Message);
            }
            else if (ex is DbEntityValidationException)
            {
                var dbex = ex as DbEntityValidationException;
                foreach (var error in dbex.EntityValidationErrors.SelectMany(result => result.ValidationErrors))
                    Exceptions.Add(new ArgumentException(error.ErrorMessage, error.PropertyName));
            }
            else
                Exceptions.Add(ex);

        }
        #endregion

        #region Logging & Instrumentation
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoggingProviders.Current.ActionExecuting(filterContext);
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LoggingProviders.Current.ActionExecuted(filterContext);
            base.OnActionExecuted(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            LoggingProviders.Current.OnException(filterContext);
            base.OnException(filterContext);
        }
        #endregion

        #region Base Actions
        protected Query.Query GetQueryById(Type modelType)
        {
            var context = ModelAssemblies.GetContext(modelType);
            var source = context.Set(modelType);
            var query = QueryParser.Parse(modelType, source, Request["q"]);
            return query;
        }

        protected Query.Query GetDefaultQueryOf(Type modelType)
        {
            var context = ModelAssemblies.GetContext(modelType);
            var source = context.Set(modelType);
            var query = QueryParser.Parse(modelType, source);
            return query;
        }

        protected ActionResult CreateInstanceOf(Type modelType, Method method, Func<object, ActionResult> onSuccess, Func<Exception, ActionResult> onException)
        {
            using (var context = ModelAssemblies.GetContext(modelType))
            {
                try
                {
                    var instance = method.Invoke(null);
                    var set = context.Set(modelType);
                    set.Add(instance);
                    context.SaveChanges();
                    return onSuccess(instance);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return onException(ex);
                }
            }
        }

        protected static object GetInstanceOf(Type modelType, object key, ModelDescriptor descriptor)
        {
            var context = ModelAssemblies.GetContext(modelType);
            var set = context.Set(modelType);
            //context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.LazyLoadingEnabled = true;
            var instance = set.Find(Convert.ChangeType(key, descriptor.KeyProperty.PropertyType));
            //context.Configuration.ProxyCreationEnabled = true;
            return instance;
        }

        protected ActionResult EditInstanceOf(Type modelType, Model model, Func<object, ActionResult> onSuccess, Func<Exception, ActionResult> onException, Func<ActionResult> onNotFound)
        {
            using (var context = ModelAssemblies.GetContext(modelType))
            {
                try
                {
                    if (model.Instance == null)
                        return onNotFound();

                    var set = context.Set(modelType);
                    var attached = set.Attach(model.Instance);

                    if (attached == null)
                        return onNotFound();

                    context.Entry(attached).State = EntityState.Modified;
                    context.SaveChanges();
                    return onSuccess(attached);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return onException(ex);
                }
            }
        }

        protected ActionResult DeleteInstanceOf(Type modelType, object key, Func<ActionResult> onSuccess, Func<Exception, ActionResult> onException, Func<ActionResult> onNotFound)
        {
            using (var context = ModelAssemblies.GetContext(modelType))
            {
                try
                {
                    var set = context.Set(modelType);
                    var descriptor = new ModelDescriptor(ModelMappingManager.FindByType(modelType));
                    var instance = set.Find(Convert.ChangeType(key, descriptor.KeyProperty.PropertyType));

                    if (instance == null)
                        return onNotFound();

                    set.Remove(instance);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return onException(ex);
                }
                return onSuccess();
            }
        }

        protected ActionResult ExecuteMethodOf(Type modelType, Method model, Func<ActionResult> onSuccess, Func<object, ActionResult> onSuccessWithReturn, Func<Exception, ActionResult> onException, Func<ActionResult> onNotFound, string key = null)
        {
            using (var context = ModelAssemblies.GetContext(modelType))
            {
                try
                {
                    object @return;
                    if (!model.Descriptor.Method.IsStatic)
                    {
                        var descriptor = new ModelDescriptor(ModelMappingManager.FindByType(modelType));
                        var set = context.Set(modelType);
                        var instance = set.Find(Convert.ChangeType(key, descriptor.KeyProperty.PropertyType));

                        if (instance == null)
                            return onNotFound();

                        @return = model.Invoke(instance);
                        context.SaveChanges();

                        if (@return == instance)
                            @return = null;
                    }
                    else
                        @return = model.Invoke(null);

                    if (@return != null)
                        return onSuccessWithReturn(@return);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return onException(ex);
                }

                return onSuccess();
            }
        }

        protected object ParseResult(Method model, object @return)
        {
            if (@return != null)
            {
                var method = model.Descriptor.Method as MethodInfo;
                if (method != null && method.ReturnType != typeof(void))
                {
                    var collectionType = method.ReturnType.GetInterface("IEnumerable`1");
                    if (collectionType != null)
                    {
                        var returnType = collectionType.GetGenericArguments()[0];
                        if (returnType != null && ModelMappingManager.Exists(returnType))
                        {
                            var mapping = ModelMappingManager.FindByType(returnType);
                            var descriptor = new ModelDescriptor(mapping);
                            return new ModelCollection(returnType, descriptor, (IEnumerable)@return);
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}