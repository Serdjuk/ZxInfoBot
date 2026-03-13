using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading;

namespace ZxInfoBot
{
    public static class SharedHttpClient
    {
        public static readonly HttpClient Client;

        static SharedHttpClient()
        {
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxConnectionsPerServer = 10
            };

            Client = new HttpClient(handler)
            {
                Timeout = Timeout.InfiniteTimeSpan
            };

            Client.DefaultRequestHeaders.UserAgent.ParseAdd("ZxInfoBot/1.0");
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}