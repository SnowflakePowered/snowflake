using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform
{
    public class PlatformControllerPorts : IPlatformControllerPorts
    {
        public string Port1 { get; set; }
        public string Port2 { get; set; }
        public string Port3 { get; set; }
        public string Port4 { get; set; }
        public string Port5 { get; set; }
        public string Port6 { get; set; }
        public string Port7 { get; set; }
        public string Port8 { get; set; }

        internal static IPlatformControllerPorts ParseControllerPorts(IDictionary<string, string> portsDictionary)
        {
            var controllerPorts = new PlatformControllerPorts();
            if (portsDictionary.ContainsKey("port1"))
            {
                controllerPorts.Port1 = portsDictionary["port1"];
            }
            if (portsDictionary.ContainsKey("port2"))
            {
                controllerPorts.Port2 = portsDictionary["port2"];
            }
            if (portsDictionary.ContainsKey("port3"))
            {
                controllerPorts.Port3 = portsDictionary["port3"];
            }
            if (portsDictionary.ContainsKey("port4"))
            {
                controllerPorts.Port4 = portsDictionary["port4"];
            }
            if (portsDictionary.ContainsKey("port5"))
            {
                controllerPorts.Port5 = portsDictionary["port5"];
            }
            if (portsDictionary.ContainsKey("port6"))
            {
                controllerPorts.Port6 = portsDictionary["port6"];
            }
            if (portsDictionary.ContainsKey("port7"))
            {
                controllerPorts.Port7 = portsDictionary["port7"];
            }
            if (portsDictionary.ContainsKey("port8"))
            {
                controllerPorts.Port8 = portsDictionary["port8"];
            }
            return controllerPorts;
        }
    }
}
