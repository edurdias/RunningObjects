namespace RunningObjects.MVC.Workflow
{
    public interface IWorkflowBuilder
    {
        string Key { get; }
        void Configure(IWorkflow workflow);
    }
}