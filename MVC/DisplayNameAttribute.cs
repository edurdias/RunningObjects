using System;

namespace RunningObjects.MVC
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