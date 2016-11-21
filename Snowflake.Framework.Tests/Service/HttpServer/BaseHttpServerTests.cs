using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Service.HttpServer.Tests
{
    /*public*/ class BaseHttpServerTests
    {
        [Fact]
        public void BaseHttpServerCreation_Test()
        {
            Assert.NotNull(new FakeHttpServer());
        }
        [Fact]
        public void BaseHttpServerStart_Test()
        {
            IBaseHttpServer server = new FakeHttpServer();
            server.StartServer();
            using (WebClient client = new WebClient())
            {
                Assert.Equal(string.Empty, client.DownloadString("http://localhost:25566"));
            }
        }
        [Fact]
        public void BaseHttpServerStop_Test()
        {
            IBaseHttpServer server = new FakeHttpServer();
            bool unableToDownload = false;
            server.StartServer();
            server.StopServer();
            using (WebClient client = new WebClient())
            {
                try
                {
                    Assert.Equal(string.Empty, client.DownloadString("http://localhost:25566"));
                }
                catch (WebException)
                {
                    unableToDownload = true;
                }
            }
            Assert.True(unableToDownload);
        }
    }
    class FakeHttpServer : BaseHttpServer
    {
        public FakeHttpServer() : base(25566) { }

        protected async override Task Process(Mono.Net.HttpListenerContext context)
        {
            var writer = new StreamWriter(context.Response.OutputStream);
            writer.Write(string.Empty);
            writer.Flush();
            context.Response.OutputStream.Close();
        }
    }
   
}
