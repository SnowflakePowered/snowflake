using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using Newtonsoft.Json;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.RootProvider;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;

namespace Snowflake.Support.Remoting.GraphQl.Servers
{
    internal class GraphQlServer : WebModuleBase
    {
        /// <inheritdoc/>
        public override string Name => "Snowflake GraphQL Remoting";

        /// <summary>
        /// The chuck size for sending files
        /// </summary>
        private const int chunkSize = 8 * 1024;
        public GraphQlServer(GraphQlExecuterProvider provider)
        {
            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, async (context, token) =>
            {
                context.NoCache();
                context.Response.ContentType = "application/json";
                var requestBody = context.RequestBody();
                var request = JsonConvert.DeserializeObject<GraphQlRequest>(requestBody);

                // Super-hacky workaround abusing Task.Run to make it run in a separate thread if nescessary.
                var result = await Task.Run(async () => await provider.ExecuteRequestAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
                string str = provider.Write(result);
                var buffer = new MemoryStream(Encoding.UTF8.GetBytes(str.ToString())).Compress();
                context.Response.AddHeader("Content-Encoding", "gzip");
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.StatusCode = result.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
                await GraphQlServer.WriteToOutputStream(context, buffer.Length, buffer, 0).ConfigureAwait(false);
                return true;
            });
        }

        // ripped from EmbedIO StaticFilesModule
        private static async Task WriteToOutputStream(HttpListenerContext context, long byteLength, Stream buffer, int lowerByteIndex)
        {
            var streamBuffer = new byte[chunkSize];
            var sendData = 0;
            var readBufferSize = chunkSize;

            while (true)
            {
                if (sendData + chunkSize > byteLength)
                {
                    readBufferSize = (int)(byteLength - sendData);
                }

                buffer.Seek(lowerByteIndex + sendData, SeekOrigin.Begin);
                var read = buffer.Read(streamBuffer, 0, readBufferSize);

                if (read == 0)
                {
                    break;
                }

                sendData += read;
                await context.Response.OutputStream.WriteAsync(streamBuffer, 0, readBufferSize).ConfigureAwait(false);
            }
        }
    }
}
