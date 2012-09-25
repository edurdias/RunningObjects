using System;
using System.Collections.Generic;
using RunningObjects.MVC.Configuration;

namespace RunningObjects.MVC.Query
{
    public class QueryConfigurationBuilder : IDisposable
    {
        private QueryConfigurationBuilder()
        {
            KeywordEvaluators = KeywordEvaluators ?? new Dictionary<string, Func<Query, object>>();
        }

        public QueryConfigurationBuilder(ConfigurationBuilder configuration)
            : this()
        {
            Configuration = configuration;
        }

        public ConfigurationBuilder Configuration { get; set; }

        public Dictionary<string, Func<Query, object>> KeywordEvaluators { get; private set; }

        public void RemoveKeywordEvaluator(string key)
        {
            if (KeywordEvaluators.ContainsKey(key))
                KeywordEvaluators.Remove(key);
        }

        internal void ParseKeywords(Query query)
        {
            foreach (var evaluator in KeywordEvaluators)
            {
                var value = evaluator.Value(query);

                if (query.Where.Expression.Contains(evaluator.Key))
                {
                    var parameterName = string.Format("@{0}", query.Parameters.Length);
                    query.Where.Expression = query.Where.Expression.Replace(evaluator.Key, parameterName);
                    var parameters = new List<object>(query.Parameters) {value};
                    query.Parameters = parameters.ToArray();
                }

                for (var i = 0; i < query.Parameters.Length; i++)
                {
                    var parameter = query.Parameters[i];
                    if (parameter != null && parameter.ToString() == evaluator.Key)
                        query.Parameters[i] = value;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}