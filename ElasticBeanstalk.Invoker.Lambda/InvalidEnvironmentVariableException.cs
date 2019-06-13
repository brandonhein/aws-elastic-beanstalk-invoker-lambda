using System;

namespace ElasticBeanstalk.Invoker.Lambda
{
    public class InvalidEnvironmentVariableException : Exception
    {
        public InvalidEnvironmentVariableException() : base("Environment Variables are required for this lambda to function properly")
        { }

        public InvalidEnvironmentVariableException Required(string key)
        {
            base.Data.Add($"Required-Variable:{key}", null);
            return this;
        }
    }
}
