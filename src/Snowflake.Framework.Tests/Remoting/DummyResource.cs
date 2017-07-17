using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting
{
    [Resource("game", ":echo")]
    [Parameter(typeof(string), "echo")]
    public class DummyResource : Resource
    {
        [Endpoint(EndpointVerb.Read)]
        public string Get()
        {
            return "Hello World!";
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(string), "echoText")]
        public string Echo(string echo, string echoText)
        {
            return echo + echoText;
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(int), "echo")]
        [Parameter(typeof(string), "echoText")]
        [Parameter(typeof(string), "someOther")]
        // this will never be picked up by the resource parser as 'echo: int' conflicts with 'echo: string'
        public string Echo(int echo, string echoText, string someOther)
        {
            return echo + echoText + someOther;
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(string), "echoText")]
        [Parameter(typeof(string), "someOther")]
        public string Echo(string echo, string echoText, string someOther)
        {
            return echo + echoText + someOther;
        }

        [Endpoint(EndpointVerb.Create)]
        [Parameter(typeof(string), "echoTextMisMatched")]
        [Parameter(typeof(string), "someOther")]
        public string EchoMisMatched(int echo, string echoTextMisMatched, string someOther)
        {
            return echo + echoTextMisMatched + someOther;
        }
    }
}
