using System;
using System.Linq;

namespace RunningObjects.Data
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T Add(T item);
        T Update(T item);
        T Remove(T item);
        bool SaveChanges();
        T Find(params object[] keyValues);
        IQueryable All();
    }
}