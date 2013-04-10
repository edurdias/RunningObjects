using System;
using System.Reflection;

namespace RunningObjects.Core.Workflow
{
    public interface IWorkflowActivity
    {
        IWorkflow Workflow { get; set; }
        string Key { get; set; }
        IWorkflowActivity NextActivity { get; set; }
        IWorkflowActivity PreviousActivity { get; set; }
        MethodBase Method { get; set; }
        Func<object, object> Binding { get; set; }
        IWorkflowConditionalActivity ThenIf(Func<object, bool> condition, Action<IWorkflowActivity> ifTrue, Action<IWorkflowActivity> @else);

        IWorkflowActivity Then<TResult>(Func<TResult> method);
        IWorkflowActivity Then<T1, TResult>(Func<T1, TResult> method);
        IWorkflowActivity Then<T1, T2, TResult>(Func<T1, T2, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method);

        IWorkflowActivity Then(Action method);
        IWorkflowActivity Then<T1>(Action<T1> method);
        IWorkflowActivity Then<T1, T2>(Action<T1, T2> method);
        IWorkflowActivity Then<T1, T2, T3>(Action<T1, T2, T3> method);
        IWorkflowActivity Then<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method);

        IWorkflowActivity Then<TResult>(Func<TResult> method, Func<object,object> binding);
        IWorkflowActivity Then<T1, TResult>(Func<T1, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, TResult>(Func<T1, T2, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method, Func<object, object> binding);

        IWorkflowActivity Then(Action method, Func<object, object> binding);
        IWorkflowActivity Then<T1>(Action<T1> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2>(Action<T1, T2> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3>(Action<T1, T2, T3> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method, Func<object, object> binding);
        IWorkflowActivity Then<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method, Func<object, object> binding);
    }
}