using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Extensibility;
using Snowflake.Support.Remoting.GraphQl.RootProvider;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Swan;

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
        public GraphQlServer(GraphQlExecuterProvider provider, ILogger logger)
        {
            Terminal.Settings.DisplayLoggingMessageType = LogMessageType.None;
            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, async (context, token) =>
            {
                context.NoCache();
                context.Response.ContentType = "application/json";
                context.Response.AddHeader("Content-Encoding", "gzip");
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token");
                var requestBody = context.RequestBody();
                try
                {
                    var request = JsonConvert.DeserializeObject<GraphQlRequest>(requestBody);

                    logger.Info($"Received GraphQL Request.");
                    // Super-hacky workaround abusing Task.Run to make it run in a separate thread if nescessary.
                    var result = await Task.Run(async () => await provider.ExecuteRequestAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
                    string str = provider.Write(result);
                    var buffer = await new MemoryStream(Encoding.UTF8.GetBytes(str.ToString())).CompressAsync();
                    context.Response.StatusCode = result.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
                    await GraphQlServer.WriteToOutputStream(context, buffer.Length, buffer, 0).ConfigureAwait(false);
                    if (result.Errors?.Any() == true)
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.Warn($"Error occurred when processing GraphQL request {error.Message} from exception {error.InnerException}");
                        }
                    }
                    logger.Info($"Processed GraphQL Request.");
                }
                catch
                {

                }
                return true;
            });
        }

        // ripped from EmbedIO StaticFilesModule
        private static async Task WriteToOutputStream(IHttpContext context, long byteLength, Stream buffer, int lowerByteIndex)
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
