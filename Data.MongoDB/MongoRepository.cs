using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace RunningObjects.Data.MongoDB
{
    public abstract class MongoRepository<T> : IRepository<T> where T : class, IMongoDocument
    {

        private readonly string connectionString;

        protected MongoRepository(string collectionName, string connectionStringName = null)
        {
            CollectionName = collectionName;
            connectionString = connectionStringName != null && ConfigurationManager.ConnectionStrings[connectionStringName] != null
                                   ? ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString
                                   : null;
        }

        private string CollectionName { get; set; }

		public MongoCollection<T> Collection
		{
			get { return GetDatabase().GetCollection<T>(CollectionName); }
		}

		protected IQueryable<T> AsQueryable()
		{
			return Collection.AsQueryable();
		}

		public T Find(params object[] keyValues)
		{
			return Collection.FindOneByIdAs<T>(keyValues[0].ToString());
		}

		public IQueryable<T> All()
		{
			return AsQueryable();
		}

		public IQueryable<T> All(Func<T, bool> expression)
		{
			return AsQueryable().Where(expression).AsQueryable();
		}

		public T Add(T item)
		{
			Collection.Save(item);
			return item;
		}

		public void Add(IEnumerable<T> items)
		{
			Collection.InsertBatch(items);
		}

		public T Update(T item)
		{
			Collection.Save(item);
			return item;
		}

		public void Update(IEnumerable<T> items)
		{
			foreach (var item in items)
				Update(item);
		}

		public T Remove(T item)
		{
			Collection.Remove(Query<T>.EQ(i => i.Id, item.Id));
			return item;
		}

		public void Remove(IEnumerable<T> items)
		{
			foreach (var item in items)
				Remove(item);
		}

		public bool SaveChanges()
		{
			return true;
		}

		public void Dispose()
		{

		}

        private MongoServer GetServer()
        {
            return !string.IsNullOrEmpty(connectionString)
                       ? new MongoClient(connectionString).GetServer()
                       : new MongoClient().GetServer();
        }

        private MongoDatabase GetDatabase()
        {
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            if (string.IsNullOrEmpty(dbName))
                dbName = "runningobjects";
            return GetServer().GetDatabase(dbName);
        }
    }
}