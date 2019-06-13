using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.TestUtilities;

namespace ElasticBeanstalk.Invoker.Lambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void Function_test()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            var upperCase = function.Handler("hello world", context);

            Assert.Equal("HELLO WORLD", upperCase);
        }
    }
}
