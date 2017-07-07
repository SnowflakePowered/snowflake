using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Test
{
    [Verb("test")]
    public class ApiTestCommandOptions
    {
        [Option('p', "port", HelpText = "API Port")]
        public int Path { get; set; }
    }

    public class ApiTestCommand : ICommand<ApiTestCommandOptions>
    {
        public async Task<int> Execute(ApiTestCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.PostRequest(client.GetServiceUrl(""), null);
            if (response.Error.Code != null) {
                Console.WriteLine("Service is up!");
                return 0;
            }
            Console.WriteLine("Service is down!");
            return 1;
        }
    }
}
