using System;
using System.Collections;
using System.Collections.Generic;
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
                if (!string.IsNullOrEmpty(argex.ParamName))
                    ModelState.AddModelError(argex.ParamName, argex.Message);
                else
                    Exceptions.Add(argex);
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
            var mapping = ModelMappingManager.MappingFor(modelType);
            var source = mapping.Configuration.Repository().All();
            return source != null
                ? QueryParser.Parse(modelType, source, Request["q"])
                : QueryParser.Empty(modelType);
        }

        protected Query.Query GetDefaultQueryOf(Type modelType)
        {
            var mapping = ModelMappingManager.MappingFor(modelType);
            var source = mapping.Configuration.Repository().All();
	        return QueryParser.Parse(modelType, source);
        }

        protected ActionResult CreateInstanceOf(Type modelType, Method method, Func<object, ActionResult> onSuccess, Func<Exception, ActionResult> onException)
        {
            try
            {
                var mapping = ModelMappingManager.MappingFor(modelType);
                using (var repository = mapping.Configuration.Repository())
                {
                    var instance = method.Invoke(null);
                    repository.Add(instance);
                    repository.SaveChanges();
                    return onSuccess(instance);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return onException(ex);
            }
        }

        protected static object GetInstanceOf(Type modelType, object key, ModelDescriptor descriptor)
        {
            return ModelMappingManager.MappingFor(modelType).Configuration.Repository().Find(GetKeyValues(key, descriptor));
        }

        private static object GetKeyValues(object key, ModelDescriptor descriptor)
        {
            var keyProperty = descriptor.KeyProperty;
            var keyValues = keyProperty.PropertyType == typeof(Guid)
                                ? Guid.Parse(key.ToString())
                                : Convert.ChangeType(key, keyProperty.PropertyType);
            return keyValues;
        }

        protected ActionResult EditInstanceOf(Type modelType, Model model, Func<object, ActionResult> onSuccess, Func<Exception, ActionResult> onException, Func<ActionResult> onNotFound)
        {
            var mapping = ModelMappingManager.MappingFor(modelType);
            using (var repository = mapping.Configuration.Repository())
            {
                try
                {
                    if (model.Instance == null)
                        return onNotFound();

                    var updated = repository.Update(model.Instance);
                    if (updated == null)
                        return onNotFound();

                    repository.SaveChanges();
                    return onSuccess(updated);
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
            var mapping = ModelMappingManager.MappingFor(modelType);
            using (var repository = mapping.Configuration.Repository())
            {
                try
                {
                    var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(modelType));
                    var instance = repository.Find(GetKeyValues(key, descriptor));

                    if (instance == null)
                        return onNotFound();

                    repository.Remove(instance);
                    repository.SaveChanges();
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
            var mapping = ModelMappingManager.MappingFor(modelType);
            using (var repository = mapping.Configuration.Repository())
            {
                try
                {
                    object @return;
                    if (!model.Descriptor.Method.IsStatic)
                    {
                        var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(modelType));
                        var instance = repository.Find(GetKeyValues(key, descriptor));

                        if (instance == null)
                            return onNotFound();

                        @return = model.Invoke(instance);
                        instance = repository.Update(instance);
                        repository.SaveChanges();

                        if (@return == instance)
                            @return = null;
                    }
                    else
                        @return = model.Invoke(null);

                    foreach (var parameter in model.Parameters.Where(p => (p.IsModel || p.IsModelCollection) && p.Value != null))
                    {
                        var paramRepository = parameter.UnderliningModel.Descriptor.ModelMapping.Configuration.Repository();
                        if (parameter.IsModelCollection)
                        {

                            //TODO:Update parameters from collection
                            //foreach (var paramItem in parameter.ToModelCollection().Items)
                            //{
                            //    paramRepository.Update(paramItem.Instance);
                            //}
                        }
                        else
                            paramRepository.Update(parameter.Value);
                        paramRepository.SaveChanges();
                    }


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
                            var mapping = ModelMappingManager.MappingFor(returnType);
                            var descriptor = new ModelDescriptor(mapping);
                            return new ModelCollection(returnType, descriptor, (IEnumerable)@return);
                        }
                    }

                    if (method.ReturnType == typeof(Redirect))
                        return @return;
                }
            }
            return null;
        }
        #endregion
    }
}