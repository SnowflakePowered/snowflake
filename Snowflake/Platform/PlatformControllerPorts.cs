using System;
using System.Collections.Generic;

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
        public string GetPort(int portNumber)
        {
            switch (portNumber)
            {
                case 1:
                    return this.Port1;
                case 2:
                    return this.Port2;
                case 3:
                    return this.Port3;
                case 4:
                    return this.Port4;
                case 5:
                    return this.Port5;
                case 6:
                    return this.Port6;
                case 7:
                    return this.Port7;
                case 8:
                    return this.Port8;
                default:
                    throw new ArgumentOutOfRangeException("Snowflake only supports ports 1 to 8");
            }
        }
        public string this[int portNumber] => this.GetPort(portNumber);

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
