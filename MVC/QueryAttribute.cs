using System;
using RunningObjects.MVC.Query;

namespace RunningObjects.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class QueryAttribute : Attribute
    {
        private object[] parameters;
        private string[] includes;

        public string Id { get; set; }

        public string Name { get; set; }

        public string Select { get; set; }

        public string Where { get; set; }

        public string OrderBy { get; set; }

        //public string GroupBy { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string[] Includes
        {
            get
            {
                return includes ?? new string[] { };
            }
            set
            {
                includes = value;
            }
        }

        public object[] Parameters
        {
            get
            {
                return parameters ?? new object[] { };
            }
            set
            {
                parameters = value;
            }
        }

        public bool IsDefault { get; set; }

        public int CacheDuration { get; set; }

        public bool Paging { get; set; }

        public int PageSize { get; set; }

        public IQueryFilter Filter { get; set; }

        public QueryAttribute()
        {
        }

        public QueryAttribute(Type queryFilterType)
        {
            var filterTypeName = typeof (IQueryFilter).FullName;
            if (queryFilterType.GetInterface(filterTypeName, true) == null)
                throw new ArgumentException(string.Format("The type {0} does not implement the interface {1}.", queryFilterType.FullName, filterTypeName));

            Filter = (IQueryFilter) Activator.CreateInstance(queryFilterType);
        }
    }
}