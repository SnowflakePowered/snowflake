using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Game
{
    [Verb("add-file")]
    public class CreateFileCommandOptions
    {
        [Option('g',"game", HelpText = "Game guid", Required = true)]
        public string Game { get; set; }
        [Option('p', "path", HelpText = "Path", Required = true)]
        public string Path { get; set; }
        [Option('m', "mimetype", HelpText = "MimeType", Required = true)]
        public string MimeType { get; set; }
    }

    public class CreateFileCommand : ICommand<CreateFileCommandOptions>
    {
        public async Task<int> Execute(CreateFileCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.PostRequest(client.GetServiceUrl("games", options.Game, "files"),
                new {
                    path = options.Path,
                    mimetype = options.MimeType
                });
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            Console.WriteLine(response.Status.Code);
            return 1;
        }
    }
}
