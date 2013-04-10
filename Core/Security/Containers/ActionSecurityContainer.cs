namespace RunningObjects.Core.Security.Containers
{
    public class ActionSecurityContainer<T> : SecurityPolicyContainer<T> where T : class
    {
        public ActionSecurityContainer(ITypeSecurityConfiguration<T> configuration, RunningObjectsAction action)
            : base(configuration)
        {
            Action = action;
        }

        protected RunningObjectsAction Action { get; set; }
    }
}