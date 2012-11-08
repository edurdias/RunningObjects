using System.Collections.Generic;
using RunningObjects.MVC.Configuration;

namespace RunningObjects.MVC.Workflow
{
    public class WorkflowConfigurationBuilder
    {
        private readonly Dictionary<string, IWorkflow> workflows = new Dictionary<string, IWorkflow>();

        internal WorkflowConfigurationBuilder(ConfigurationBuilder configuration)
        {
            Configuration = configuration;
        }

        public ConfigurationBuilder Configuration { get; private set; }

        public IWorkflow For(IWorkflowBuilder builder)
        {
            var workflow = For(builder.Key);
            builder.Configure(workflow);
            return workflow;
        }

        public IWorkflow For(string key)
        {
            if (!Exists(key))
                workflows.Add(key, new SequentialWorkflow(key, this));
            return workflows[key];
        }

        public bool Exists(string key)
        {
            return workflows.ContainsKey(key);
        }
    }
}