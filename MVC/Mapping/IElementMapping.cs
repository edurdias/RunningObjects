namespace RunningObjects.MVC.Mapping
{
    public interface IElementMapping
    {
        string ID { get; }
        string Name { get; }
        IElementMapping Parent { get; }
        bool Visible { get; set; }
    }
}