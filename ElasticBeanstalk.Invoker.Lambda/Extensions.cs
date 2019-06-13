namespace ElasticBeanstalk.Invoker.Lambda
{
    public static class Extensions
    {
        public static string GetContentType(this string payload)
        {
            string contentType = "text/plain";

            if (!string.IsNullOrEmpty(payload))
            {
                if (payload.StartsWith("<"))
                {
                    contentType = "application/xml";
                }

                if (payload.StartsWith("{") || payload.StartsWith("["))
                {
                    contentType = "application/json";
                }
            }

            return contentType;
        }
    }
}
