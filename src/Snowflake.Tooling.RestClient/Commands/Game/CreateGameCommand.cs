using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands.Game
{
    [Verb("add-game")]
    public class CreateGameCommandOptions
    {
        [Option('t',"title", HelpText = "The Game Title", Required = true)]
        public string Title { get; set; }
        [Option('p', "platform", HelpText = "The Stone Platform ID of the game", Required = true)]
        public string PlatformID { get; set; }
    }

    public class CreateGameCommand : ICommand<CreateGameCommandOptions>
    {
        public async Task<int> Execute(CreateGameCommandOptions options)
        {
            var client = new ApiClient();
            var response = await client.PostRequest(client.GetServiceUrl("games"), new
            {
                title =  options.Title,
                platform =  options.PlatformID
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
