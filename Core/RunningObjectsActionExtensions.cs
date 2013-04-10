using System;
using System.Linq;

namespace RunningObjects.Core
{
    public static class RunningObjectsActionExtensions
    {
        public static RunningObjectsAction GetAction(this RunningObjectsAction action, string actionName)
        {
            if (Enum.GetNames(typeof(RunningObjectsAction)).Any(name => name == actionName))
                return (RunningObjectsAction)Enum.Parse(typeof(RunningObjectsAction), actionName);
            return RunningObjectsAction.Execute;
        }
    }
}