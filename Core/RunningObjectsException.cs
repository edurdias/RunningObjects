using System;

namespace RunningObjects.Core
{
    public class RunningObjectsException : Exception
    {
        public RunningObjectsException(string message)
            : base(message)
        {
        }
    }
}