using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO.Compression;
using System.IO;

namespace Snowflake.Tooling.RestClient
{
    public class ApiClient
    {
        private readonly int port;
        private HttpClient client;
        private readonly string url;
        public ApiClient(int port = 9696)
        {
            this.port = 9696;
            this.client = new HttpClient();
            this.url = $"http://localhost:{port}";
        }

        public string GetServiceUrl(string serviceName, params string[] args)
        {
            return String.Join('/', args.Prepend(serviceName).Prepend(this.url));
        }
        public async Task<dynamic> PostRequest(string serviceUrl, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(serviceUrl, content);
            var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Position = 0;
            string contentStr;
            try
            {
                var compressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
                var decompressedStream = new MemoryStream();
                await compressedStream.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;
                contentStr = await new StreamReader(decompressedStream).ReadToEndAsync();
            }catch(InvalidDataException)
            {
                responseStream.Position = 0;
                contentStr = await new StreamReader(responseStream).ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<dynamic>(contentStr);
        }

        public async Task<dynamic> PutRequest(string serviceUrl, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(serviceUrl, content);
            var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Position = 0;
            string contentStr;
            try
            {
                var compressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
                var decompressedStream = new MemoryStream();
                await compressedStream.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;
                contentStr = await new StreamReader(decompressedStream).ReadToEndAsync();
            }catch(InvalidDataException)
            {
                responseStream.Position = 0;
                contentStr = await new StreamReader(responseStream).ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<dynamic>(contentStr);
        }

         public async Task<dynamic> GetRequest(string serviceUrl)
        {
            HttpResponseMessage response = await client.GetAsync(serviceUrl);
            var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Position = 0;
            string contentStr;
            try
            {
                var compressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
                var decompressedStream = new MemoryStream();
                await compressedStream.CopyToAsync(decompressedStream);
                decompressedStream.Position = 0;
                contentStr = await new StreamReader(decompressedStream).ReadToEndAsync();
            }catch(InvalidDataException)
            {
                responseStream.Position = 0;
                contentStr = await new StreamReader(responseStream).ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<dynamic>(contentStr);
        }
    }
}
