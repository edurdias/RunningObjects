namespace RunningObjects.Core.Mapping
{
    public interface IMappingElement
    {
        string ID { get; }
        string Name { get; }
        IMappingElement Parent { get; }
        bool Visible { get; set; }
    }
}