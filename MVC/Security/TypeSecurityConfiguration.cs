using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RunningObjects.MVC.Security.Containers;

namespace RunningObjects.MVC.Security
{
    public class TypeSecurityConfiguration<T> : ITypeSecurityConfiguration<T>
    {
        private AnythingSecurityContainer<T> anything;
        private ConstructorSecurityContainer<T> constructor;
        private ActionSecurityContainer<T> view;
        private ActionSecurityContainer<T> edit;
        private ActionSecurityContainer<T> delete;
        private ActionSecurityContainer<T> index;
        private readonly Dictionary<string, ISecurityPolicyContainer<T>> executes = new Dictionary<string, ISecurityPolicyContainer<T>>();

        public ISecurityPolicyContainer<T> OnAnything()
        {
            return anything ?? (anything = new AnythingSecurityContainer<T>());
        }

        public ISecurityPolicyContainer<T> OnCreate()
        {
            return constructor ?? (constructor = new ConstructorSecurityContainer<T>());
        }

        public ISecurityPolicyContainer<T> OnIndex()
        {
            return index ?? (index = new ActionSecurityContainer<T>(RunningObjectsAction.Index));
        }

        public ISecurityPolicyContainer<T> OnView()
        {
            return view ?? (view = new ActionSecurityContainer<T>(RunningObjectsAction.View));
        }

        public ISecurityPolicyContainer<T> OnEdit()
        {
            return edit ?? (edit = new ActionSecurityContainer<T>(RunningObjectsAction.Edit));
        }

        public ISecurityPolicyContainer<T> OnDelete()
        {
            return delete ?? (delete = new ActionSecurityContainer<T>(RunningObjectsAction.Delete));
        }

        public IEnumerable<ISecurityPolicyContainer<T>> AllExecutions()
        {
            return executes.Values.ToList();
        }

        internal ISecurityPolicyContainer<T> OnExecute(MethodInfo method)
        {
            var key = method.ToString();
            if (!executes.ContainsKey(key))
                executes.Add(key, new MethodSecurityContainer<T>(method));
            return executes[key];
        }

        #region OnExecute

        #region Static Functions

        public ISecurityPolicyContainer<T> OnExecuteWithReturn(Func<object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1>(Func<T1, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2>(Func<T1, T2, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3>(Func<T1, T2, T3, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4>(Func<T1, T2, T3, T4, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute(Action expression)
        {
            return OnExecute(expression.Method);
        }

        #endregion

        #region Static Actions
        public ISecurityPolicyContainer<T> OnExecute<T1>(Action<T1> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2>(Action<T1, T2> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3>(Action<T1, T2, T3> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4>(Action<T1, T2, T3, T4> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> expression)
        {
            return OnExecute(expression.Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> expression)
        {
            return OnExecute(expression.Method);
        }
        #endregion

        #region Instance Functions
        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1>(Func<T, Func<T1, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn(Func<T, Func<object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2>(Func<T, Func<T1, T2, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3>(Func<T, Func<T1, T2, T3, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4>(Func<T, Func<T1, T2, T3, T4, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T, Func<T1, T2, T3, T4, T5, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T, Func<T1, T2, T3, T4, T5, T6, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, object>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute(Func<T, Action> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        #endregion

        #region Instance Actions
        public ISecurityPolicyContainer<T> OnExecute<T1>(Func<T, Action<T1>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2>(Func<T, Action<T1, T2>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3>(Func<T, Action<T1, T2, T3>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4>(Func<T, Action<T1, T2, T3, T4>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5>(Func<T, Action<T1, T2, T3, T4, T5>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6>(Func<T, Action<T1, T2, T3, T4, T5, T6>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }

        public ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression)
        {
            return OnExecute(expression(Activator.CreateInstance<T>()).Method);
        }
        #endregion

        #endregion

        public ISecurityPolicyContainer<T> FindPolicyContainer(ControllerContext controllerContext)
        {
            var actionName = controllerContext.RouteData.Values["action"].ToString();
            var action = (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
            switch (action)
            {
                case RunningObjectsAction.Create:
                    if (constructor != null)
                        return OnCreate();
                    break;
                case RunningObjectsAction.Index:
                    if (index != null)
                        return OnIndex();
                    break;
                case RunningObjectsAction.View:
                    if (view != null)
                        return OnView();
                    break;
                case RunningObjectsAction.Edit:
                    if (edit != null)
                        return OnEdit();
                    break;
                case RunningObjectsAction.Delete:
                    if (delete != null)
                        return OnDelete();
                    break;
                case RunningObjectsAction.Execute:
                    var methodName = controllerContext.RouteData.Values["methodName"].ToString();
                    var methodIndex = Convert.ToInt32(controllerContext.RouteData.Values["index"].ToString());
                    var isStatic = !controllerContext.RouteData.Values.ContainsKey("key");
                    var containers = AllExecutions().OfType<MethodSecurityContainer<T>>().Where(m => m.Method.Name == methodName).ToList();
                    containers.RemoveAll(m => m.Method.IsStatic != isStatic);
                    if(methodIndex < containers.Count)
                        return containers.ElementAt(methodIndex);
                    break;
            }
            return OnAnything();
        }
    }
}