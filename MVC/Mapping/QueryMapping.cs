namespace RunningObjects.MVC.Mapping
{
    public class QueryMapping : IElementMapping
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public TypeMapping Type { get; set; }

        public IElementMapping Parent
        {
            get { return Type; }
        }

        public bool Visible { get; set; }
    }
}