using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace RunningObjects.MVC
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
            var attach = Context.Set(Type).Attach(item);
            if(attach != null)
                Context.Entry(item).State = EntityState.Modified;
            return attach;
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
            return Context.Set(Type).Find(keyValues);
        }

        public IQueryable All()
        {
            return Context.Set(Type);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}