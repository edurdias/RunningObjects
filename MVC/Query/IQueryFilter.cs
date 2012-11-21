using System.Linq;

namespace RunningObjects.MVC.Query
{
    public interface IQueryFilter
    {
        IQueryable Apply(IQueryable items);
    }
}