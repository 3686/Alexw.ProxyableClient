using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Alexw.ProxyableClient.ConsoleApplication
{
    public static class HttpClientWrapper
    {
        public static string GetPlainTextResult(Uri sourceUri, Uri proxyUri)
        {
            var requestHandler = new HttpClientHandler();
            if (proxyUri != null)
            {
                requestHandler.Proxy = new WebProxy(proxyUri);
            }
            using (var client = new HttpClient(requestHandler))
            {
                try
                {
                    using (var result = client.GetAsync(sourceUri).Result)
                    {
                        if (!result.IsSuccessStatusCode)
                            throw new HttpRequestException("HttpStatusCode: " + result.StatusCode);

                        return result.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (AggregateException ae)
                {
                    var e = ae.InnerExceptions.First();
                    while (e.InnerException != null) e = e.InnerException;
                    return "Error: " + e.Message;
                }
                catch (HttpRequestException hre)
                {
                    return "Error: " + hre.GetBaseException().Message;
                }
            }
        }
    }
}