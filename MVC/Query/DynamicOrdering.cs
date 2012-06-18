using System.Linq.Expressions;

namespace RunningObjects.MVC.Query
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}