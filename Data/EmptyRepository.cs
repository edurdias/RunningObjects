using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningObjects.Data
{
	public class EmptyRepository<T> : IRepository<T> where T : class
	{
		public T Add(T item)
		{
			return item;
		}

		public void Add(IEnumerable<T> items)
		{
		}

		public T Update(T item)
		{
			return item;
		}

		public void Update(IEnumerable<T> items)
		{
			
		}

		public T Remove(T item)
		{
			return item;
		}

		public void Remove(IEnumerable<T> items)
		{
			
		}

		public bool SaveChanges()
		{
			return false;
		}

		public T Find(params object[] keyValues)
		{
			return null;
		}

		public IQueryable<T> All()
		{
			return default(IQueryable<T>);
		}

		public IQueryable<T> All(Func<T, bool> expression)
		{
			return default(IQueryable<T>);
		}

		public void Dispose()
		{

		}
	}
}