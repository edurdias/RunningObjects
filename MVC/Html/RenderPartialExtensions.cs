using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RunningObjects.MVC.Mapping;
using System.Web.Mvc.Html;

namespace RunningObjects.MVC.Html
{
    public static class RenderPartialExtensions
    {
        private static void RenderMethod<TModel>(HtmlHelper htmlHelper, MethodBase method, object key = null)
        {
            RenderMethod(htmlHelper, typeof (TModel), method, key);
        }

        private static void RenderMethod(HtmlHelper htmlHelper, Type modelType, MethodBase method, object key = null)
        {
            var typeMapping = ModelMappingManager.MappingFor(modelType);
            var methods = method.IsConstructor
                              ? typeMapping.Constructors
                              : method.IsStatic
                                    ? typeMapping.StaticMethods
                                    : typeMapping.InstanceMethods;

            var methodMapping = methods.FirstOrDefault(m => m.Method == method);
            if (methodMapping == null)
                throw new ArgumentNullException(string.Format("The method {0} was not found in type {1}.", method.Name, typeMapping.Name));
            htmlHelper.RenderAction(methodMapping.UnderlineAction.ToString(), "Presentation", GetRouteValues(method, key, methodMapping, typeMapping));
        }

        private static object GetRouteValues(MethodBase method, object key, MethodMapping methodMapping, TypeMapping typeMapping)
        {
            object routeValues = new
            {
                modelType = typeMapping.ModelType.PartialName(),
                index = methodMapping.Index
            };

            if (!method.IsConstructor)
            {
                routeValues = new
                {
                    modelType = typeMapping.ModelType.PartialName(),
                    index = methodMapping.Index,
                    methodName = methodMapping.MethodName
                };
            }

            if (!method.IsStatic && key != null)
            {
                routeValues = new
                {
                    modelType = typeMapping.ModelType.PartialName(),
                    index = methodMapping.Index,
                    methodName = methodMapping.MethodName,
                    key
                };
            }
            return routeValues;
        }

        #region Constructors
        public static void RenderPartialConstructor<TModel>(this HtmlHelper htmlHelper, int index)
        {
            RenderPartialConstructor(htmlHelper, typeof(TModel), index);
        }

        public static void RenderPartialConstructor(this HtmlHelper htmlHelper, Type modelType, int index)
        {
            var ctors = modelType.GetConstructors().Where(c => c.GetParameters().Any());
            if (ctors.Count() <= index)
                throw new IndexOutOfRangeException(string.Format("Constructor not found at index {0} for type {1}.", index, modelType.PartialName()));
            RenderMethod(htmlHelper, modelType, ctors.ElementAt(index));
        } 
        #endregion

        #region View / Edit
        public static void RenderPartialView<TModel>(this HtmlHelper htmlHelper, object key)
        {
            RenderPartialView(htmlHelper, typeof(TModel), key);
        }

        public static void RenderPartialView(this HtmlHelper htmlHelper, Type modelType, object key)
        {
            htmlHelper.RenderAction(RunningObjectsAction.View.ToString(), "Presentation", new { modelType = modelType.PartialName(), key });   
        }

        public static void RenderPartialEdit<TModel>(this HtmlHelper htmlHelper, object key)
        {
            RenderPartialView(htmlHelper, typeof(TModel), key);
        }

        public static void RenderPartialEdit(this HtmlHelper htmlHelper, Type modelType, object key)
        {
            htmlHelper.RenderAction(RunningObjectsAction.Edit.ToString(), "Presentation", new { modelType = modelType.PartialName(), key });
        } 
        #endregion


        #region Static Actions
        public static void RenderPartial<TModel, T1>(this HtmlHelper htmlHelper, Action<T1> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2>(this HtmlHelper htmlHelper, Action<T1, T2> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3>(this HtmlHelper htmlHelper, Action<T1, T2, T3> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4>(this HtmlHelper htmlHelper, Action<T1, T2, T3, T4> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5>(this HtmlHelper htmlHelper, Action<T1, T2, T3, T4, T5> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6>(this HtmlHelper htmlHelper, Action<T1, T2, T3, T4, T5, T6> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6, T7>(this HtmlHelper htmlHelper, Action<T1, T2, T3, T4, T5, T6, T7> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6, T7, T8>(this HtmlHelper htmlHelper, Action<T1, T2, T3, T4, T5, T6, T7, T8> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }
        #endregion

        #region Instance Actions
        public static void RenderPartial<TModel, T1>(this HtmlHelper htmlHelper, Func<TModel, Action<T1>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3, T4>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3, T4, T5>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3, T4, T5, T6>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6, T7>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3, T4, T5, T6, T7>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartial<TModel, T1, T2, T3, T4, T5, T6, T7, T8>(this HtmlHelper htmlHelper, Func<TModel, Action<T1, T2, T3, T4, T5, T6, T7, T8>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }
        #endregion

        #region Static Functions
        public static void RenderPartialWithReturn<TModel, T1>(this HtmlHelper htmlHelper, Func<T1, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2>(this HtmlHelper htmlHelper, Func<T1, T2, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3>(this HtmlHelper htmlHelper, Func<T1, T2, T3, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4>(this HtmlHelper htmlHelper, Func<T1, T2, T3, T4, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5>(this HtmlHelper htmlHelper, Func<T1, T2, T3, T4, T5, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6>(this HtmlHelper htmlHelper, Func<T1, T2, T3, T4, T5, T6, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6, T7>(this HtmlHelper htmlHelper, Func<T1, T2, T3, T4, T5, T6, T7, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6, T7, T8>(this HtmlHelper htmlHelper, Func<T1, T2, T3, T4, T5, T6, T7, T8, object> expression)
        {
            RenderMethod<TModel>(htmlHelper, expression.Method);
        }
        #endregion

        #region Instance Functions
        public static void RenderPartialWithReturn<TModel, T1>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, T4, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, T4, T5, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, T4, T5, T6, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6, T7>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, T4, T5, T6, T7, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }

        public static void RenderPartialWithReturn<TModel, T1, T2, T3, T4, T5, T6, T7, T8>(this HtmlHelper htmlHelper, Func<TModel, Func<T1, T2, T3, T4, T5, T6, T7, T8, object>> expression, object key)
        {
            RenderMethod<TModel>(htmlHelper, expression(Activator.CreateInstance<TModel>()).Method, key);
        }
        #endregion

    }
}
