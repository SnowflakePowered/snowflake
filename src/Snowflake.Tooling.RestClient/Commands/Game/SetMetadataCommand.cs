using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Game
{
    [Verb("set-metadata")]
    public class SetMetadataCommandOptions
    {
        [Option('g',"game", HelpText = "Game Guid", Required = true)]
        public string Guid { get; set; }

        [Option('k', "key", HelpText = "The Metadata Key", Required = true)]
        public string Key { get; set; }

        [Option('v', "value", HelpText = "The Metadata Value", Required = true)]
        public string Value { get; set; }
    }

    public class SetMetadataCommand : ICommand<SetMetadataCommandOptions>
    {
        public async Task<int> Execute(SetMetadataCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.PutRequest(client.GetServiceUrl("games", options.Guid, "metadata", options.Key), new
            {
                value = options.Value
            });
            if (response.Error != null)
            {
                Console.WriteLine("Something went wrong!");
                Console.WriteLine(JsonConvert.SerializeObject(response));
                return 1;
            }
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            return 1;
        }
    }
}
