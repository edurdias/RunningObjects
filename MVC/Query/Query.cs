using System;
using System.Data.Entity;
using System.Linq;
using RunningObjects.MVC.Caching;
using RunningObjects.MVC.Mapping;

namespace RunningObjects.MVC.Query
{
    public class Query
    {
        private string cacheKey;
        public const int DefaultPageSize = 25;

        public Query(Type modelType, IQueryable source)
        {
            ModelType = modelType;
            Source = source;
        }

        protected IQueryable Source { get; set; }

        public Type ModelType { get; set; }

        public string Id { get; set; }

        public Select Select { get; set; }

        public Where Where { get; set; }

        public OrderBy OrderBy { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Include { get; set; }

        public object[] Parameters { get; set; }

        public string Key { get; set; }

        public string Text { get; set; }

        public bool IsEmpty { get; internal set; }

        public int CacheDuration { get; set; }

        public bool IsCacheable
        {
            get { return CacheDuration > 0; }
        }

        private string CacheKey
        {
            get
            {
                if (string.IsNullOrEmpty(cacheKey))
                    cacheKey = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", ModelType.PartialName(), Select, Where, OrderBy, Skip, Take);
                return cacheKey;
            }
        }

        public bool Paging { get; set; }

        public int PageSize { get; set; }

        public IQueryable Execute(bool? flush = null)
        {
            if (IsCacheable && (!flush.HasValue || !flush.Value))
            {
                if (!CacheProvider.Current.Contains(CacheKey))
                    CacheProvider.Current.Add(CacheKey, GetAppliedQuery(), CacheDuration);
                return CacheProvider.Current.Get(CacheKey) as IQueryable;
            }
            return GetAppliedQuery();
        }

        private IQueryable GetAppliedQuery()
        {
            if (Where != null)
                Source = (Parameters != null && Parameters.Count() > 0)
                    ? Source.Where(Where.Expression, Parameters)
                    : Source.Where(Where.Expression);

            if (OrderBy != null && OrderBy.Elements.Any())
            {
                var orderBy = string.Empty;
                foreach (var element in OrderBy.Elements)
                    orderBy += element.Key + " " + element.Value + ",";
                Source = Source.OrderBy(orderBy.Substring(0, orderBy.Length - 1));
            }
            else
            {
                var mapping = ModelMappingManager.FindByType(ModelType);
                var descriptor = new ModelDescriptor(mapping);
                Source = Source.OrderBy(descriptor.KeyProperty.Name + " Asc");
            }


            if (Skip != null)
                Source = Source.Skip(Skip.Value);

            if (Take != null)
                Source = Source.Take(Take.Value);

            if (!string.IsNullOrEmpty(Include))
                Source = Source.Include(Include);

            var data = (Select != null && Select.Properties.Any())
                           ? Source.Select(string.Format("new({0})", Select))
                           : Source;
            return data;
        }
    }
}