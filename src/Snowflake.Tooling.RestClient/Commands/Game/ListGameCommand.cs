using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Game
{

    [Verb("list-game")]
    public class ListGameCommandOptions
    {
        [Option('g',"game", HelpText = "Game UUID")]
        public string Game { get; set; }
    }

    public class ListGameCommand : ICommand<ListGameCommandOptions>
    {
        public async Task<int> Execute(ListGameCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.GetRequest(client.GetServiceUrl("games"));
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
