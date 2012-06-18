namespace RunningObjects.MVC.Logging
{
    public class LoggingProviders
    {
        private static LoggingProvider current;

        public static LoggingProvider Current
        {
            get
            {
                if (current == null)
                    current = new DefaultLoggingProvider();
                return current;
            }
            set { current = value; }
        }
    }
}