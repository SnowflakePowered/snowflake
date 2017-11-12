using GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Snowflake.Support.Remoting.GraphQl.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Unosquare.Labs.EmbedIO;
using Unosquare.Net;

namespace Snowflake.Support.Remoting.GraphQl.Servers
{
    public class GraphQlServer : WebModuleBase
    {
        public override string Name => "Snowflake REST Remoting";
        /// <summary>
        /// The chuck size for sending files
        /// </summary>
        private const int chunkSize = 8 * 1024;
        public GraphQlServer(GraphQlExecuterProvider provider)
        {
            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) =>
            {
                context.NoCache();
                context.Response.ContentType = "application/json";
                var requestBody = context.RequestBody();
                var request = JsonConvert.DeserializeObject<GraphQlRequest>(requestBody);
                var result = provider.ExecuteRequestAsync(request).Result;
                string str = provider.Write(result);
                var buffer = new MemoryStream(Encoding.UTF8.GetBytes(str.ToString())).Compress();
                context.Response.AddHeader("Content-Encoding", "gzip");
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.StatusCode = result.Errors?.Any() == true ? (int)System.Net.HttpStatusCode.BadRequest : (int)System.Net.HttpStatusCode.OK;
                GraphQlServer.WriteToOutputStream(context, buffer.Length, buffer, 0);
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
    }
}
