using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC.Persistence
{
    public class InMemoryRepository : IRepository<object>
    {
        private static readonly Dictionary<Type, IList> internalRepositories = new Dictionary<Type, IList>();

        public InMemoryRepository(Type modelType)
        {
            ModelType = modelType;
            if (!internalRepositories.ContainsKey(modelType))
            {
                var prototype = typeof(List<>);
                var genericType = prototype.MakeGenericType(modelType);
                internalRepositories.Add(modelType, Activator.CreateInstance(genericType) as IList);
            }
        }

        protected Type ModelType { get; set; }

        public object Add(object item)
        {
            InternalRepository.Add(item);
            return item;
        }

        public object Update(object item)
        {
            var original = InternalRepository.Cast<object>().FirstOrDefault(o => o.Equals(item));
            InternalRepository.Remove(original);
            InternalRepository.Add(item);
            return item;
        }

        protected IList InternalRepository
        {
            get { return internalRepositories[ModelType]; }
        }

        public object Remove(object item)
        {
            InternalRepository.Remove(item);
            return item;
        }

        public bool SaveChanges()
        {
            return true;
        }

        public object Find(params object[] keyValues)
        {
            if (keyValues.Length >= 1)
            {
                var mapping = ModelMappingManager.MappingFor(ModelType);
                return InternalRepository.Cast<object>().FirstOrDefault(item => keyValues[0].Equals(mapping.Key.GetValue(item)));
            }
            return null;
        }

        public IQueryable All()
        {
            return InternalRepository.AsQueryable();
        }

        public void Dispose()
        {
        }
    }
}
