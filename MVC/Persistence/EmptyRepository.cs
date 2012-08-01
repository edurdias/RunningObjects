using System.Linq;

namespace RunningObjects.MVC.Persistence
{
    public class EmptyRepository<T> : IRepository<T> where T : class
    {
        public T Add(T item)
        {
            return item;
        }

        public T Update(T item)
        {
            return item;            
        }

        public T Remove(T item)
        {
            return item;
        }

        public bool SaveChanges()
        {
            return false;
        }

        public T Find(params object[] keyValues)
        {
            return null;
        }

        public IQueryable All()
        {
            return default(IQueryable);
        }

        public void Dispose()
        {
            
        }
    }
}