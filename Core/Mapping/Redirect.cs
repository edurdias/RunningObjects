using System;
using System.Linq;
using System.Reflection;

namespace RunningObjects.Core.Mapping
{
    public class Redirect
    {
        protected Redirect()
        {
        }

        protected TypeMapping Mapping { get; set; }

        public Type Type { get { return Mapping.ModelType; } }

        public RunningObjectsAction Action { get; set; }

        public int MethodIndex { get; set; }

        public object Arguments { get; set; }

        public static Redirect ToWelcome()
        {
            return new ComplexRedirect<object>
            {
                Action = RunningObjectsAction.Welcome
            };
        }

        public static ComplexRedirect<T> To<T>() where T : class
        {
            return new ComplexRedirect<T>();
        }

        public class ComplexRedirect<T> : Redirect where T : class
        {
            internal ComplexRedirect()
            {
                Mapping = ModelMappingManager.MappingFor(typeof(T));
            }

            public Redirect Index()
            {
                Action = RunningObjectsAction.Index;
                return this;
            }

            public Redirect Constructor(int index = 0)
            {
                if (Mapping.Constructors.Count() <= index)
                    throw new ArgumentOutOfRangeException("index");

                Action = RunningObjectsAction.Create;
                MethodIndex = index;
                return this;
            }

            public Redirect OnExecute(Action expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecute<T1>(Action<T1> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecute<T1, T2>(Action<T1, T2> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecute<T1, T2, T3>(Action<T1, T2, T3> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecute(Func<T, Action> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1>(Func<T, Action<T1>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1, T2>(Func<T, Action<T1, T2>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1, T2, T3>(Func<T, Action<T1, T2, T3>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1, T2, T3, T4>(Func<T, Action<T1, T2, T3, T4>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1, T2, T3, T4, T5>(Func<T, Action<T1, T2, T3, T4, T5>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecute<T1, T2, T3, T4, T5, T6>(Func<T, Action<T1, T2, T3, T4, T5, T6>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn(Func<Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn<T1>(Func<T1, Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn<T1, T2>(Func<T1, T2, Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3>(Func<T1, T2, T3, Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Object> expression)
            {
                return OnExecute(expression.Method);
            }
            
            public Redirect OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Object> expression)
            {
                return OnExecute(expression.Method);
            }

            public Redirect OnExecuteWithReturn(Func<T, Func<Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1>(Func<T, Func<T1, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1, T2>(Func<T, Func<T1, T2, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3>(Func<T, Func<T1, T2, T3, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3, T4>(Func<T, Func<T1, T2, T3, T4, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T, Func<T1, T2, T3, T4, T5, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }

            public Redirect OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T, Func<T1, T2, T3, T4, T5, T6, Object>> expression, T instance)
            {
                var action = expression(instance);
                var key = Mapping.Key.GetValue(instance);
                return OnExecute(action.Method, key);
            }
            
            private Redirect OnExecute(MethodBase method, object key = null)
            {
                var methods = method.IsStatic ? Mapping.StaticMethods : Mapping.InstanceMethods;
                var mapping = methods.FirstOrDefault(m => m.Method == method);

                if (mapping == null)
                    throw new ArgumentException(string.Format("Method {0} not found.", method.Name), "method");

                Action = mapping.UnderlineAction;
                MethodIndex = mapping.Index;
                Arguments = method.IsStatic
                                ? (object)new { methodName = mapping.MethodName, index = mapping.Index }
                                : new { methodName = mapping.MethodName, index = mapping.Index, key };
                return this;
            }

            
        }
    }
}