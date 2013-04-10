namespace RunningObjects.Core.Workflow
{
    public interface IWorkflowBuilder
    {
        string Key { get; }
        void Configure(IWorkflow workflow);
    }
}