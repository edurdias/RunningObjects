using System;

namespace RunningObjects.Core
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Class | AttributeTargets.Method)]
    public class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute(string displayName) 
            : base(displayName)
        {
             
        }
    }
}