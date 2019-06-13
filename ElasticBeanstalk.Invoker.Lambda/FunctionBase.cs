using System;
using System.IO;
using System.Net;

namespace ElasticBeanstalk.Invoker.Lambda
{
    public class FunctionBase
    {
        public HttpStatusCode Invoke(string url, string method, string body)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = method.ToUpper();

            if (method.ToUpper() != "GET".ToUpper())
            {
                webRequest.ContentType = body.GetContentType();

                using (var stream = new StreamWriter(webRequest.GetRequestStream()))
                {
                    stream.Write(body);
                    stream.Flush();
                    stream.Close();
                }
            }

            try
            {
                return GetResponseCode(webRequest);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    return GetResponseCode(webRequest);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return HttpStatusCode.InternalServerError;
        }

        private HttpStatusCode GetResponseCode(HttpWebRequest request)
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return response.StatusCode;
            }
        }
    }
}
