using System;

namespace RunningObjects.Core.Workflow
{
    public interface IWorkflow
    {
        string Key { get; set; }
        WorkflowConfigurationBuilder Configuration { get; set; }

        IWorkflowActivity StartActivity { get; set; }
        IWorkflowActivity StartAt<TResult>(Func<TResult> method);
        IWorkflowActivity StartAt<T1, TResult>(Func<T1, TResult> method);
        IWorkflowActivity StartAt<T1, T2, TResult>(Func<T1, T2, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method);

        IWorkflowActivity StartAt(Action method);
        IWorkflowActivity StartAt<T1>(Action<T1> method);
        IWorkflowActivity StartAt<T1, T2>(Action<T1, T2> method);
        IWorkflowActivity StartAt<T1, T2, T3>(Action<T1, T2, T3> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method);
        IWorkflowActivity StartAt<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method);
    }
}