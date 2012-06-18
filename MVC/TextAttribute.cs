using System;

namespace RunningObjects.MVC
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TextAttribute : Attribute
    {
    }
}