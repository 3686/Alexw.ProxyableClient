using System;
using System.Configuration;

namespace Alexw.ProxyableClient.ConsoleApplication
{
    public static class Configuration
    {
        public static Settings ValidateConfiguration()
        {
            var rawSourceUri = ConfigurationManager.AppSettings["Source.Uri"];
            if (!Uri.IsWellFormedUriString(rawSourceUri, UriKind.Absolute))
            {
                Console.WriteLine("Invalid Source.Uri setting - must be absolute URI e.g., http://example.com");
                return null;
            }

            var rawProxyUri = ConfigurationManager.AppSettings["Proxy.Uri"];
            if (!Uri.IsWellFormedUriString(rawProxyUri, UriKind.Absolute))
            {
                Console.WriteLine("Invalid Proxy.Uri setting - must be absolute URI e.g., http://104.131.14.247:8080");
                return null;
            }

            return new Settings(rawProxyUri, rawSourceUri);
        }
    }
}