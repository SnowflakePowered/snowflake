using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.IO;
using System.Web;
using System.Net;
using Xunit;
namespace Snowflake.Service.HttpServer.Tests
{
    public class BaseHttpServerTests
    {
        [Fact]
        public void BaseHttpServerCreation_Test()
        {
            Assert.NotNull(new FakeHttpServer());
        }
        [Fact]
        public void BaseHttpServerStart_Test()
        {
            var server = new FakeHttpServer();
            server.StartServer();
            using (WebClient client = new WebClient())
            {
                Assert.Equal(String.Empty, client.DownloadString("http://localhost:25566"));
            }
        }
        [Fact]
        public void BaseHttpServerStop_Test()
        {
            var server = new FakeHttpServer();
            bool unableToDownload = false;
            server.StartServer();
            server.StopServer();
            using (WebClient client = new WebClient())
            {
                try
                {
                    Assert.Equal(String.Empty, client.DownloadString("http://localhost:25566"));
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
            writer.Write(String.Empty);
            writer.Flush();
            context.Response.OutputStream.Close();
        }
    }
   
}
