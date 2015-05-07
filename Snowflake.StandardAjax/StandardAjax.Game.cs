using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Extensions;
using Snowflake.Utility;
using Snowflake.Emulator;
using Snowflake.Emulator.Configuration;
using Newtonsoft.Json;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetGameResults(IJSRequest request)
        {
            string filename = request.GetParameter("filename");
            string platform = request.GetParameter("platform");
            return new JSResponse(request, this.CoreInstance.LoadedPlatforms[platform].GetScrapeEngine().GetGameResults(filename));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "scraper", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetGameResultsUsingScraper(IJSRequest request)
        {
            string filename = request.GetParameter("filename");
            string platform = request.GetParameter("platform");
            string scraperId = request.GetParameter("scraper");

            return new JSResponse(request, this.CoreInstance.LoadedPlatforms[platform].GetScrapeEngine(scraperId).GetGameResults(filename));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "resultid", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetGameInfo(IJSRequest request)
        {
            string resultid = request.GetParameter("resultid");
            string platform = request.GetParameter("platform");
            string filename = request.GetParameter("filename");
            return new JSResponse(request, this.CoreInstance.LoadedPlatforms[platform].GetScrapeEngine().GetGameInfo(resultid, filename));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "gameinfo", ParameterType = AjaxMethodParameterType.ObjectParameter)]
        public IJSResponse AddGameInfo(IJSRequest request)
        {
            string gameinfo_pre = request.GetParameter("gameinfo");
            IGameInfo game = GameInfo.FromJson(JsonConvert.DeserializeObject(gameinfo_pre));
            this.CoreInstance.GameDatabase.AddGame(game);
            return new JSResponse(request, "added " + game.FileName, true);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetGame(IJSRequest request)
        {
            string id = request.GetParameter("id");
            return new JSResponse(request, this.CoreInstance.GameDatabase.GetGameByUUID(id));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetGamesByPlatform(IJSRequest request)
        {
            string platform = request.GetParameter("platform");
            return new JSResponse(request, this.CoreInstance.GameDatabase.GetGamesByPlatform(platform));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetAllGames(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.GameDatabase.GetAllGames());
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetAllGamesSorted(IJSRequest request)
        {
            IList<IGameInfo> games = this.CoreInstance.GameDatabase.GetAllGames();
            IDictionary<string, List<IGameInfo>> sortedGames = this.CoreInstance.LoadedPlatforms.ToDictionary(platform => platform.Key, platform => new List<IGameInfo>());
            foreach(IGameInfo game in games)
            {
                if (sortedGames.ContainsKey(game.PlatformID))
                {
                    sortedGames[game.PlatformID].Add(game);
                }
            }
            return new JSResponse(request, sortedGames);
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetFlags(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            return new JSResponse(request, bridge.ConfigurationFlags);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetFlagValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IGameInfo game = this.CoreInstance.GameDatabase.GetGameByUUID(id);
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetValue(game, flag.Key, flag.Type));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetFlagValues(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IGameInfo game = this.CoreInstance.GameDatabase.GetGameByUUID(id);
            IDictionary<string, dynamic> flags = bridge.ConfigurationFlags.ToDictionary(flag => flag.Value.Key, flag => bridge.ConfigurationFlagStore.GetValue(game, flag.Value.Key, flag.Value.Type));
            return new JSResponse(request, flags);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetFlagValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            string key = request.GetParameter("key");
            string value = request.GetParameter("value");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IGameInfo game = this.CoreInstance.GameDatabase.GetGameByUUID(id);
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            dynamic castedValue = bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type);
            switch (flag.Type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    castedValue = Boolean.Parse(value);
                    break;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    castedValue = Int32.Parse(value);
                    break;
                case ConfigurationFlagTypes.SELECT_FLAG:
                    castedValue = Int32.Parse(value);
                    break;
            }
            bridge.ConfigurationFlagStore.SetValue(game, flag.Key, castedValue, flag.Type);
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetValue(game, flag.Key, flag.Type));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetFlagDefaultValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            string value = request.GetParameter("value");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            dynamic castedValue = bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type);
            switch (flag.Type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    castedValue = Boolean.Parse(value);
                    break;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    castedValue = Int32.Parse(value);
                    break;
                case ConfigurationFlagTypes.SELECT_FLAG:
                    castedValue = Int32.Parse(value);
                    break;
            }
            bridge.ConfigurationFlagStore.SetDefaultValue(flag.Key, castedValue, flag.Type);
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type));
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetFlagDefaultValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetFlagDefaultValues(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IDictionary<string, dynamic> flags = bridge.ConfigurationFlags.ToDictionary(flag => flag.Value.Key, flag => bridge.ConfigurationFlagStore.GetDefaultValue(flag.Value.Key, flag.Value.Type));
            return new JSResponse(request, flags);
        }
            
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse StartGame(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            IEmulatorBridge bridge = this.CoreInstance.PluginManager.LoadedEmulators[emulator];
            IGameInfo gameInfo = this.CoreInstance.GameDatabase.GetGameByUUID(id);
            bridge.StartRom(gameInfo);
            return new JSResponse(request, "Game Started " + gameInfo.UUID);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse HaltRunningGames(IJSRequest request)
        {
            foreach (IEmulatorBridge emulator in this.CoreInstance.PluginManager.LoadedEmulators.Values)
            {
                emulator.ShutdownEmulator();
            }
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse SearchGames(IJSRequest request)
        {
            //todo implement this with a searching algo
            return new JSResponse(request, null);
        }
    }
}
