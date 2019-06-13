using System;

namespace ElasticBeanstalk.Invoker.Lambda
{
    public static class LambdaEnvironment
    {
        public static bool HasEnvironmentVariable(string variable)
        {
            try
            {
                return !string.IsNullOrEmpty(GetEnvironmentVariable(variable));
            }
            catch
            { }

            return false;
        }

        public static string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public static bool TryGetEnvironmentVariable(string variable, out string result)
        {
            var resultBool = HasEnvironmentVariable(variable);
            result = resultBool ? GetEnvironmentVariable(variable) : string.Empty;

            return resultBool;
        }
    }
}
