using System.Linq;

namespace RunningObjects.Core.Query
{
    public interface IQueryFilter
    {
        IQueryable Apply(IQueryable items);
    }
}