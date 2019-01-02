using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Scraping.TheGamesDb.Utility
{
    internal static class StaticWebClient
    {
        private static HttpClient Client { get; }

        static StaticWebClient()
        {
            StaticWebClient.Client = new HttpClient();
        }

        public static Task DownloadAsync(string requestUri, string filename)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("Request URL can not be null");
            }

            return DownloadAsync(new Uri(requestUri), filename);
        }

        public static async Task DownloadAsync(Uri requestUri, string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("Filename can not be null");
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (
                    Stream contentStream = await (await Client.SendAsync(request)).Content.ReadAsStreamAsync(),
                    stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }
        }

        public static async Task<Stream> DownloadDataAsync(Uri requestUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                Stream contentStream = await (await Client.SendAsync(request)).Content.ReadAsStreamAsync();
                return contentStream;
            }
        }

        public static void Download(Uri requestUri, string filename)
        {
            StaticWebClient.DownloadAsync(requestUri, filename).RunSynchronously();
        }

        public static void Download(string requestUri, string filename)
        {
            StaticWebClient.DownloadAsync(requestUri, filename).RunSynchronously();
        }

        public static Stream DownloadData(Uri requestUri)
        {
            return StaticWebClient.DownloadDataAsync(requestUri).Result;
        }
    }
}
