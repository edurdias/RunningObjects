using System;

namespace RunningObjects.Core.Workflow
{
    public interface IWorkflowConditionalActivity : IWorkflowActivity 
    {
        Func<object, bool> Condition { get; set; }

        IWorkflowActivity IfTrueActivity { get; set; }

        IWorkflowActivity ElseActivity { get; set; }
    }
}