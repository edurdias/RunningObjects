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

        public IQueryable All()
        {
            return AsQueryable();
        }

        public T Add(T item)
        {
            Collection.Save(item);
            return item;
        }

        public T Update(T item)
        {
            Collection.Save(item);
            return item;
        }

        public T Remove(T item)
        {
            Collection.Remove(Query<T>.EQ(i => i.Id, item.Id));
            return item;
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