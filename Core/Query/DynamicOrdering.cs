using System.Linq.Expressions;

namespace RunningObjects.Core.Query
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}