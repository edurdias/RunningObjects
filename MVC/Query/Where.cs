namespace RunningObjects.MVC.Query
{
    public class Where
    {
        public Where(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; set; }
    }
}