using System;

namespace RunningObjects.MVC
{
    public class RunningObjectsException : Exception
    {
        public RunningObjectsException(string message)
            : base(message)
        {
        }
    }
}