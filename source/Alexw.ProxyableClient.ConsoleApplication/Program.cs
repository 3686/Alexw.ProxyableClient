using System;
using System.Threading;

namespace Alexw.ProxyableClient.ConsoleApplication
{
    public class Program
    {
        private static readonly ManualResetEventSlim ApplicationExitSignal = new ManualResetEventSlim(false);

        public static void Main(string[] args)
        {
            var settings = Configuration.ValidateConfiguration();
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
                var resultWithoutProxy = HttpClientWrapper.GetPlainTextResult(settings.SourceUri, null);
                Console.WriteLine("Result: {0}", resultWithoutProxy);
                Console.WriteLine();

                Console.WriteLine("Fetching {0} with the proxy. ProxyUri: {1}", settings.SourceUri, settings.ProxyUri);
                var resultWithProxy = HttpClientWrapper.GetPlainTextResult(settings.SourceUri, settings.ProxyUri);
                Console.WriteLine("Result: {0}", resultWithProxy);
                Console.WriteLine();

                Console.ReadLine();
            }

            Console.WriteLine("Application ready to close");
            Console.ReadLine();
        }

        private static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ApplicationExitSignal.Set();
        }
    }
}
