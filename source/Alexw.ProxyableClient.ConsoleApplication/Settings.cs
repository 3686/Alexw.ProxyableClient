using System;

namespace Alexw.ProxyableClient.ConsoleApplication
{
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
}