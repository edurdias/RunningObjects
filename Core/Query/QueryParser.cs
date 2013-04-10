using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RunningObjects.Core.Mapping;

namespace RunningObjects.Core.Query
{
    public static class QueryParser
    {
        public static Query Empty(Type modelType)
        {
            var prototype = typeof (List<>);
            var genericType = prototype.MakeGenericType(modelType);
            var source = Activator.CreateInstance(genericType) as IEnumerable;
            return new Query(modelType, source.AsQueryable());
        }

        public static Query Parse(Type modelType, IQueryable source)
        {
            var queries = modelType.GetCustomAttributes(true).OfType<QueryAttribute>();
            var attr = queries.Count() > 1 ? queries.FirstOrDefault(q => q.IsDefault) : queries.FirstOrDefault();
            return Parse(modelType, source, attr);
        }

        public static Query Parse(Type modelType, IQueryable source, string id)
        {
            var queries = modelType.GetCustomAttributes(true).OfType<QueryAttribute>();
            var attr = queries.FirstOrDefault(q => q.Id == id) ?? queries.FirstOrDefault();
            return Parse(modelType, source, attr);
        }

        public static Query Parse(Type modelType, IQueryable source, QueryAttribute attr)
        {
            var query = new Query(modelType, source);
            if (attr != null)
            {
                query.Select = ParseSelect(modelType, attr);
                query.Where = ParseWhere(attr);
                query.OrderBy = ParseOrderBy(modelType, attr);
                //query.GroupBy = attr.GroupBy;
                query.Skip = ParseSkip(attr);
                query.Take = ParseTake(attr);
                query.CacheDuration = attr.CacheDuration;
                query.Includes = attr.Includes;
                query.Parameters = attr.Parameters;
                query.Paging = attr.Paging;
                query.PageSize = attr.PageSize > 0 ? attr.PageSize : Query.DefaultPageSize;
                query.Filter = attr.Filter;
                query.Id = attr.Id;
            }
            else
                query.IsEmpty = true;
            return query;
        }

        private static Select ParseSelect(Type modelType, QueryAttribute spec)
        {
            var select = new Select();
            var descriptor = new ModelDescriptor(ModelMappingManager.MappingFor(modelType));
            var properties = descriptor.GetProperties().OfType<PropertyDescriptor>();

            //select.Properties.AddRange(properties);
            if (!string.IsNullOrEmpty(spec.Select))
            {
                select.Properties.Clear();
                var propertyNames = spec.Select.Split(Select.Separator);
                foreach (var propertyName in propertyNames)
                {
                    var property = properties.FirstOrDefault(p => p.Name == propertyName);
                    if (property != null)
                        select.Properties.Add(property);
                }
            }
            return select;
        }

        private static Where ParseWhere(QueryAttribute spec)
        {
            return !string.IsNullOrEmpty(spec.Where) ? new Where(spec.Where) : null;
        }

        private static OrderBy ParseOrderBy(Type modelType, QueryAttribute spec)
        {
            if (!string.IsNullOrEmpty(spec.OrderBy))
            {
                var orderBy = new OrderBy();
                foreach (var element in spec.OrderBy.Split(OrderBy.ElementSeparator))
                {
                    if (OrderBy.IsValid(element))
                    {
                        var parts = element.Split(OrderBy.Separator);
                        var propertyName = parts[0];
                        var sortingDirection = parts[1];

                        if (string.IsNullOrEmpty(sortingDirection) || (!sortingDirection.ToLowerInvariant().Equals("asc") && !sortingDirection.ToLowerInvariant().Equals("desc")))
                            throw new InvalidOperationException(string.Format("Invalid sorting direction for the OtherBy clause of {0}'s query.", modelType.FullName));

                        orderBy.Elements.Add(propertyName,
                                             sortingDirection.ToLowerInvariant().Equals("asc")
                                                 ? "ascending"
                                                 : "descending");
                    }
                }
                return orderBy;
            }
            return null;
        }

        private static int? ParseSkip(QueryAttribute spec)
        {
            return spec.Skip > 0 ? spec.Skip : (int?)null;
        }

        private static int? ParseTake(QueryAttribute spec)
        {
            return spec.Take > 0 ? spec.Take : (int?)null;
        }
    }
}
