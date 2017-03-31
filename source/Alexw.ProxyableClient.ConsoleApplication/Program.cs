using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;

namespace Alexw.ProxyableClient.ConsoleApplication
{

    public class Program
    {
        private static readonly ManualResetEventSlim ApplicationExitSignal = new ManualResetEventSlim(false);

        public static void Main(string[] args)
        {
            var settings = ValidateConfiguration();
            if (settings == null)
            {
                Console.WriteLine("Invalid configuration - exiting");
                Console.ReadLine();
                return;
            }

            Console.CancelKeyPress += HandleCancelKeyPress;

            while (!ApplicationExitSignal.IsSet)
            {
                Console.WriteLine("Fetching {0} without the proxy", settings.SourceUri);
                var resultWithoutProxy = GetPlainTextResult(settings.SourceUri, null);
                Console.WriteLine("Result: {0}" + resultWithoutProxy);
                Console.WriteLine();

                Console.WriteLine("Fetching {0} with the proxy. ProxyUri: {1}", settings.SourceUri, settings.ProxyUri);
                var resultWithProxy = GetPlainTextResult(settings.SourceUri, settings.ProxyUri);
                Console.WriteLine("Result: {0}" + resultWithProxy);
                Console.WriteLine();

                Console.ReadLine();
            }

            Console.WriteLine("Application ready to close");
            Console.ReadLine();
        }

        private static Settings ValidateConfiguration()
        {
            var rawSourceUri = ConfigurationManager.AppSettings["Source.Uri"];
            if (!Uri.IsWellFormedUriString(rawSourceUri, UriKind.Absolute))
            {
                Console.WriteLine("Invalid Source.Uri setting - must be absolute URI e.g., http://example.com");
                ApplicationExitSignal.Set();
                return null;
            }

            var rawProxyUri = ConfigurationManager.AppSettings["Proxy.Uri"];
            if (!Uri.IsWellFormedUriString(rawProxyUri, UriKind.Absolute))
            {
                Console.WriteLine("Invalid Proxy.Uri setting - must be absolute URI e.g., http://104.131.14.247:8080");
                ApplicationExitSignal.Set();
                return null;
            }

            return new Settings(rawProxyUri, rawSourceUri);
        }

        public class Settings
        {
            public Settings(string proxy, string source)
            {
                ProxyUri = new Uri(proxy);
                SourceUri = new Uri(source);
            }
            public Uri ProxyUri { get; }
            public Uri SourceUri { get; }
        }

        private static string GetPlainTextResult(Uri sourceUri, Uri proxyUri)
        {
            var settings = new HttpClientHandler();
            if (proxyUri != null)
            {
                settings.Proxy = new WebProxy(proxyUri);
            }
            using (var client = new HttpClient())
            {
                var result = client.GetAsync(sourceUri).Result;
                if (!result.IsSuccessStatusCode)
                {
                    return "(Not Found)";
                }
                return result.Content.ReadAsStringAsync().Result;
            }
        }

        private static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ApplicationExitSignal.Set();
        }
    }
}
