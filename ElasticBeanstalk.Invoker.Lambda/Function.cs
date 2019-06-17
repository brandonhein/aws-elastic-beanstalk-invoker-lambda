using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ElasticBeanstalk.Invoker.Lambda
{
    public class Function : FunctionBase
    {
        private readonly string _ebUrl;
        private readonly string _method;
        private string _body;
        public Function()
        {
            LambdaEnvironment.TryGetEnvironmentVariable("url", out _ebUrl);
            LambdaEnvironment.TryGetEnvironmentVariable("method", out _method);
            LambdaEnvironment.TryGetEnvironmentVariable("body", out _body);

            if (string.IsNullOrEmpty(_ebUrl) || string.IsNullOrEmpty(_method))
            {
                var ex = new InvalidEnvironmentVariableException();
                if (string.IsNullOrEmpty(_ebUrl))
                {
                    ex.Required("url");
                }
                if (string.IsNullOrEmpty(_method))
                {
                    ex.Required("method");
                }
                throw ex;
            }
        }

        public string Handler(object input, ILambdaContext context)
        {
            if (string.IsNullOrEmpty(_body))
            {
                _body = input.ToString();
            }

            var result = base.Invoke(_ebUrl, _method, _body);
            return result.ToString();
        }
    }
}
