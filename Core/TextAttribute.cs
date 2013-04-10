using System;

namespace RunningObjects.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TextAttribute : Attribute
    {
    }
}