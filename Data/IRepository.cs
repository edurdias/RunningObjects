using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningObjects.Data
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T Add(T item);
	    void Add(IEnumerable<T> items);

        T Update(T item);
        void Update(IEnumerable<T> items);

        T Remove(T item);
	    void Remove(IEnumerable<T> items);

        bool SaveChanges();

        T Find(params object[] keyValues);

        IQueryable<T> All();
	    IQueryable<T> All(Func<T, bool> expression);
    }
}