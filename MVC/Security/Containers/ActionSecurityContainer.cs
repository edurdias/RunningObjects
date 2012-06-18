namespace RunningObjects.MVC.Security.Containers
{
    public class ActionSecurityContainer<T> : SecurityPolicyContainer<T>
    {
        public ActionSecurityContainer(RunningObjectsAction action)
        {
            Action = action;
        }

        protected RunningObjectsAction Action { get; set; }
    }
}