using System;
using System.Reflection;

namespace RunningObjects.Core.Workflow
{
    internal class SequentialWorkflowActivity : IWorkflowActivity
    {
        internal SequentialWorkflowActivity(IWorkflow workflow, MethodBase method, IWorkflowActivity previous)
        {
            Workflow = workflow;
            Method = method;
            PreviousActivity = previous;
            Key = Guid.NewGuid().ToString("N");
        }

        private SequentialWorkflowActivity(IWorkflow workflow, MethodBase method, SequentialWorkflowActivity previous, Func<object, object> binding)
            :this(workflow, method, previous)
        {
            Binding = binding;
        }

        public IWorkflow Workflow { get; set; }
        public string Key { get; set; }
        public IWorkflowActivity NextActivity { get; set; }
        public IWorkflowActivity PreviousActivity { get; set; }
        public MethodBase Method { get; set; }
        public Func<object, object> Binding { get; set; }

        public IWorkflowConditionalActivity ThenIf(Func<object, bool> condition, Action<IWorkflowActivity> ifTrue, Action<IWorkflowActivity> @else)
        {
            var activity = new SequentialWorkflowConditionalActivity(Workflow, condition, this);

            var ifTrueActivity = new SequentialWorkflowActivity(Workflow, null, this);
            ifTrue(ifTrueActivity);
            activity.IfTrueActivity = ifTrueActivity.NextActivity;
            

            var elseActivity = new SequentialWorkflowActivity(Workflow, null, this);
            @else(elseActivity);
            activity.ElseActivity = elseActivity.NextActivity;

            NextActivity = activity;

            return activity;
        }

        public IWorkflowActivity Then<TResult>(Func<TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, TResult>(Func<T1, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, TResult>(Func<T1, T2, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then(Action method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1>(Action<T1> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2>(Action<T1, T2> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3>(Action<T1, T2, T3> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method)
        {
            return Then(method.Method);
        }

        public IWorkflowActivity Then<TResult>(Func<TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, TResult>(Func<T1, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, TResult>(Func<T1, T2, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then(Action method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1>(Action<T1> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2>(Action<T1, T2> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3>(Action<T1, T2, T3> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        public IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method, Func<object, object> binding)
        {
            return Then(method.Method, binding);
        }

        private IWorkflowActivity Then(MethodBase method)
        {
            return (NextActivity = new SequentialWorkflowActivity(Workflow, method, this));
        }

        private IWorkflowActivity Then(MethodBase method, Func<object,object> binding)
        {
            return (NextActivity = new SequentialWorkflowActivity(Workflow, method, this, binding));
        }
    }
}