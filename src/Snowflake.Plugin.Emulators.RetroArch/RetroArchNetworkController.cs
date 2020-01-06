using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    public class RetroArchNetworkController : IDisposable
    {
        public RetroArchNetworkController() : this(55355) { }

        public RetroArchNetworkController(int port) : this("127.0.0.1", port) { }

        public RetroArchNetworkController(string ipaddress, int port)
        {
         
            this.Client = new UdpClient();
            this.Endpoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
        }

        private UdpClient Client { get; }
        public IPEndPoint Endpoint { get; }

        private async Task SendRetroArchCommand(string command, int times, int delay)
        {
            for (int i = 0; i < times; i++)
            {
                var bytes = Encoding.ASCII.GetBytes(command);
                await this.Client.SendAsync(bytes, bytes.Length, this.Endpoint);
                await Task.Delay(delay);
            }
        }

        public Task Quit() => this.SendRetroArchCommand("QUIT", 4, 50);

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
        }
    }
}
