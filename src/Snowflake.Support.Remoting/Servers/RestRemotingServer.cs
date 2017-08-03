using Newtonsoft.Json;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unosquare.Labs.EmbedIO;
using System.Linq;
using System.Linq.Expressions;
using Snowflake.Support.Remoting.Resources;
using System.Dynamic;
using Snowflake.Support.Remoting.Framework;
using Newtonsoft.Json.Converters;
using System.IO.Compression;
using System.IO;
using Unosquare.Net;
using Snowflake.Remoting.Requests;
using Snowflake.Remoting.Marshalling;

namespace Snowflake.Support.Remoting.Servers
{
    public class RestRemotingServer : WebModuleBase
    {
        public override string Name => "Snowflake REST Remoting";
        /// <summary>
        /// The chuck size for sending files
        /// </summary>
        private const int chunkSize = 8 * 1024;
        public RestRemotingServer(IResourceContainer endpoints)
        {
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings; //stopgap solution
            });

            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) =>
            {
                var verb = context.RequestVerb();
                context.NoCache();
                context.Response.ContentType = "application/json";
                var split = context.RequestPath().Split('/');
                var requestPath = RestRemotingServer.ToRequestPath(context.RequestPathCaseSensitive());
                var endpointVerb = verb.ToCrud();
                var requestBody = context.RequestBody();
                var endpointArguments = requestBody != null ?
                   JsonConvert.DeserializeObject<IDictionary<string, string>>(requestBody)
                  .Select(v => new EndpointArgument(v.Key, v.Value)).Cast<IEndpointArgument>() 
                    : Enumerable.Empty<IEndpointArgument>();

                var response = endpoints.ExecuteRequest(new Request(requestPath, endpointVerb, endpointArguments));

                context.Response.StatusCode = response.Status?.Code ?? 200; 
                var str = new StringBuilder();
                str.Append(JsonConvert.SerializeObject(response));
                var buffer = new MemoryStream(Encoding.UTF8.GetBytes(str.ToString())).Compress();
                context.Response.AddHeader("Content-Encoding", "gzip");
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                RestRemotingServer.WriteToOutputStream(context, buffer.Length, buffer, 0);
                return true;
            });
        }

        //ripped from EmbedIO StaticFilesModule
        private static void WriteToOutputStream(HttpListenerContext context, long byteLength, Stream buffer,
        int lowerByteIndex)
        {
            var streamBuffer = new byte[chunkSize];
            var sendData = 0;
            var readBufferSize = chunkSize;

            while (true)
            {
                if (sendData + chunkSize > byteLength) readBufferSize = (int)(byteLength - sendData);

                buffer.Seek(lowerByteIndex + sendData, SeekOrigin.Begin);
                var read = buffer.Read(streamBuffer, 0, readBufferSize);

                if (read == 0) break;

                sendData += read;
                context.Response.OutputStream.Write(streamBuffer, 0, readBufferSize);
            }
        }

        private static RequestPath ToRequestPath(string path)
        {
            return new RequestPath(path.Split('/').Where(s => s.Length > 0).ToArray());
        }


    }
}
