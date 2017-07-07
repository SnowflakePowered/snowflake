using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Game
{
    [Verb("file")]
    public class CreateFileCommandOptions
    {
        [Option('g',"game", HelpText = "The Game Title", Required = true)]
        public string Game { get; set; }
        [Option('p', "path", HelpText = "The Stone Platform ID of the game", Required = true)]
        public string Path { get; set; }
    }

    public class CreateFileCommand : ICommand<CreateFileCommandOptions>
    {
        public async Task<int> Execute(CreateFileCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.PostRequest(client.GetServiceUrl(""), null);
            if (response.Error.Code != null)
            {
                Console.WriteLine("Service is up!");
                return 0;
            }
            Console.WriteLine("Service is down!");
            return 1;
        }
    }
}
