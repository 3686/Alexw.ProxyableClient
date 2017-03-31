using System;
using System.Net;
using System.Net.Http;

namespace Alexw.ProxyableClient.ConsoleApplication
{
    public static class HttpClientWrapper
    {
        public static string GetPlainTextResult(Uri sourceUri, Uri proxyUri)
        {
            var settings = new HttpClientHandler();
            if (proxyUri != null)
            {
                settings.Proxy = new WebProxy(proxyUri);
            }
            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(sourceUri).Result)
                {
                    return !result.IsSuccessStatusCode ? "(Not Found)" : result.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}