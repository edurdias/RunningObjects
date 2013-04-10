using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace RunningObjects.Data.EntityFramework
{
    public class EntityFrameworkRepository : IRepository<object>
    {
        public EntityFrameworkRepository(Type type, DbContext context)
        {
            Type = type;
            Context = context;
        }

        protected Type Type { get; private set; }
        protected DbContext Context { get; private set; }

        public object Add(object item)
        {
            return Context.Set(Type).Add(item);
        }

        public object Update(object item)
        {
            if(item == null)
                throw new ArgumentNullException("item");
            Context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public object Remove(object item)
        {
            return Context.Set(Type).Remove(item);
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

        public object Find(params object[] keyValues)
        {
            var item = Context.Set(Type).Find(keyValues);
            return item;
        }

        public IQueryable All()
        {
            return Context.Set(Type);
        }

        public void Dispose()
        {
        }
    }
}