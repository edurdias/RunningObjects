using System;
using RunningObjects.Data;

namespace RunningObjects.Core.Mapping.Configuration
{
    public class TypeMappingConfiguration
    {
        public TypeMappingConfiguration(Type type, AssemblyMappingConfiguration assemblyMappingConfiguration)
        {
            UnderlineType = type;
            Assembly = assemblyMappingConfiguration;
        }

        public Type UnderlineType { get; set; }
        public Func<IRepository<Object>> Repository { get; set; }
        public AssemblyMappingConfiguration Assembly { get; set; }
    }
}