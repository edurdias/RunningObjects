namespace RunningObjects.MVC.Mapping
{
    public class QueryMapping : IMappingElement
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public TypeMapping Type { get; set; }

        public IMappingElement Parent
        {
            get { return Type; }
        }

        public bool Visible { get; set; }
    }
}