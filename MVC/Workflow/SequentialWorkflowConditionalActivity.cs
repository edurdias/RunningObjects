using System;

namespace RunningObjects.MVC.Workflow
{
    internal class SequentialWorkflowConditionalActivity : SequentialWorkflowActivity, IWorkflowConditionalActivity
    {
        public SequentialWorkflowConditionalActivity(IWorkflow workflow, Func<object, bool> condition, IWorkflowActivity previousActivity)
            : base(workflow, null, previousActivity)
        {
            Condition = condition;
        }

        public Func<object, bool> Condition { get; set; }

        public IWorkflowActivity IfTrueActivity { get; set; }

        public IWorkflowActivity ElseActivity { get; set; }
    }
}