using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace RunningObjects.Data.EntityFramework
{
    public class RunningObjectsDbContext : DbContext
    {
        public RunningObjectsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected DbModelBuilder Builder { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            Builder = builder;
            base.OnModelCreating(builder);
        }

        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Deleted || entityEntry.State == EntityState.Modified;
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entry, IDictionary<object, object> items)
        {
            items = items ?? new Dictionary<object, object>();
            items.Add("state", entry.State);


            var result = entry.State == EntityState.Deleted
                             ? ValidateEntityOnDelete(entry, items)
                             : base.ValidateEntity(entry, items);
            return result;
        }

        private DbEntityValidationResult ValidateEntityOnDelete(DbEntityEntry entry, IDictionary<object, object> items)
        {
            var state = entry.State;
            entry.State = EntityState.Unchanged;
            var type = entry.Entity.GetType();

            var members = type.GetProperties().Select(p => entry.Member(p.Name));
            members.OfType<DbCollectionEntry>().ToList().ForEach(e => e.Load());
            members.OfType<DbReferenceEntry>().ToList().ForEach(e => e.Load());

            var result = base.ValidateEntity(entry, items);
            entry.State = state;
            return result;
        }
    }
}