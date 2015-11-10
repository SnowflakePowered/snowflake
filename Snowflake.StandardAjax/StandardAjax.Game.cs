using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Emulator;
using Snowflake.Emulator.Configuration;
using Snowflake.Events;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Extensions;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Scraper;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = false)]
        public IJSResponse GetGameResults(IJSRequest request)
        {
            string filename = request.GetParameter("filename");
            string platform = request.GetParameter("platform");
            var engine = this.CoreInstance.Get<IScrapeEngine>();
            var platformInfo = (platform == null) ? null : !this.CoreInstance.Platforms.ContainsKey(platform) ? null : this.CoreInstance.Platforms[platform];
            var info = engine.GetScrapableInfo(filename, platformInfo);
            return new JSResponse(request, engine.GetScrapeResults(info));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = false)]
        [AjaxMethodParameter(ParameterName = "scraper", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetGameResultsUsingScraper(IJSRequest request)
        {
            string filename = request.GetParameter("filename");
            string platform = request.GetParameter("platform");
            string scraperId = request.GetParameter("scraper");
            var engine = this.CoreInstance.Get<IScrapeEngine>();
            var platformInfo = (platform == null) ? null : !this.CoreInstance.Platforms.ContainsKey(platform) ? null : this.CoreInstance.Platforms[platform];
            var info = engine.GetScrapableInfo(filename, platformInfo);
            var scraper = this.CoreInstance.Get<IPluginManager>().Plugin<IScraper>(scraperId);
            return new JSResponse(request, engine.GetScrapeResults(info, scraper));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "result", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = false)]
        public IJSResponse GetGameInfoFromResult(IJSRequest request)
        {
            string result_pre = request.GetParameter("result");
            IGameScrapeResult result = JsonConvert.DeserializeObject<GameScrapeResult>(result_pre);
            string platform = request.GetParameter("platform");
            string filename = request.GetParameter("filename");
            var engine = this.CoreInstance.Get<IScrapeEngine>();
            var platformInfo = (platform == null) ? null : !this.CoreInstance.Platforms.ContainsKey(platform) ? null : this.CoreInstance.Platforms[platform];
            var info = engine.GetScrapableInfo(filename, platformInfo);
            return new JSResponse(request, engine.GetGameData(info, result));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "filename", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = false)]
        public IJSResponse GetGameInfo(IJSRequest request)
        {
            string platform = request.GetParameter("platform");
            string filename = request.GetParameter("filename");
            var engine = this.CoreInstance.Get<IScrapeEngine>();
            var platformInfo = (platform == null) ? null : !this.CoreInstance.Platforms.ContainsKey(platform) ? null : this.CoreInstance.Platforms[platform];
            var info = engine.GetScrapableInfo(filename, platformInfo);
            return new JSResponse(request, engine.GetGameData(info));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "gameinfo", ParameterType = AjaxMethodParameterType.ObjectParameter, Required = true)]
        public IJSResponse AddGameInfo(IJSRequest request)
        {
            string gameinfo_pre = request.GetParameter("gameinfo");
            IGameInfo game = GameInfo.FromJson(JsonConvert.DeserializeObject(gameinfo_pre));
            var gamePreAddEvent = new GamePreAddEventArgs(this.CoreInstance, game, this.CoreInstance.Get<IGameDatabase>());
            SnowflakeEventManager.EventSource.RaiseEvent(gamePreAddEvent);
            if (!gamePreAddEvent.Cancel)
            {
                game = gamePreAddEvent.GameInfo;
                this.CoreInstance.Get<IGameDatabase>().AddGame(game);
                var gameAddEvent = new GameAddEventArgs(this.CoreInstance, game, this.CoreInstance.Get<IGameDatabase>());
                SnowflakeEventManager.EventSource.RaiseEvent(gameAddEvent);

            }
            return new JSResponse(request, game, true);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetGame(IJSRequest request)
        {
            string id = request.GetParameter("id");
            return new JSResponse(request, this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetGamesByPlatform(IJSRequest request)
        {
            string platform = request.GetParameter("platform");
            return new JSResponse(request, this.CoreInstance.Get<IGameDatabase>().GetGamesByPlatform(platform));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetAllGames(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.Get<IGameDatabase>().GetAllGames());
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetAllGamesSorted(IJSRequest request)
        {
            IList<IGameInfo> games = this.CoreInstance.Get<IGameDatabase>().GetAllGames();
            IDictionary<string, List<IGameInfo>> sortedGames = this.CoreInstance.Platforms.ToDictionary(platform => platform.Key, platform => new List<IGameInfo>());
            foreach (IGameInfo game in games.Where(game => sortedGames.ContainsKey(game.PlatformID)))
            {
                sortedGames[game.PlatformID].Add(game);
            }
            foreach (List<IGameInfo> gameInfos in sortedGames.Values)
            {
                gameInfos.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));
            }
            return new JSResponse(request, sortedGames);
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetFlags(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            return new JSResponse(request, bridge.ConfigurationFlags);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetFlagValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IGameInfo game = this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id);
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetValue(game, flag.Key, flag.Type));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetFlagValues(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IGameInfo game = this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id);
            IDictionary<string, dynamic> flags = bridge.ConfigurationFlags.ToDictionary(flag => flag.Value.Key, flag => bridge.ConfigurationFlagStore.GetValue(game, flag.Value.Key, flag.Value.Type));
            return new JSResponse(request, flags);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetFlagValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            string key = request.GetParameter("key");
            string value = request.GetParameter("value");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IGameInfo game = this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id);
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            dynamic castedValue = bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type);
            switch (flag.Type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    castedValue = bool.Parse(value);
                    break;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    castedValue = int.Parse(value);
                    break;
                case ConfigurationFlagTypes.SELECT_FLAG:
                    castedValue = int.Parse(value);
                    break;
            }
            bridge.ConfigurationFlagStore.SetValue(game, flag.Key, castedValue, flag.Type);
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetValue(game, flag.Key, flag.Type));
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "values", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetFlagValues(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            string values_pre = request.GetParameter("values");
            IDictionary<string, string> values = JsonConvert.DeserializeObject<IDictionary<string, string>>(values_pre);
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IGameInfo game = this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id);
            foreach (KeyValuePair<string, string> value in values)
            {
                IConfigurationFlag flag = bridge.ConfigurationFlags[value.Key];
                dynamic castedValue = bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type);
                switch (flag.Type)
                {
                    case ConfigurationFlagTypes.BOOLEAN_FLAG:
                        castedValue = bool.Parse(value.Value);
                        break;
                    case ConfigurationFlagTypes.INTEGER_FLAG:
                        castedValue = int.Parse(value.Value);
                        break;
                    case ConfigurationFlagTypes.SELECT_FLAG:
                        castedValue = int.Parse(value.Value);
                        break;
                }
                bridge.ConfigurationFlagStore.SetValue(game, flag.Key, castedValue, flag.Type);
            }
            IDictionary<string, dynamic> flags = bridge.ConfigurationFlags.ToDictionary(flag => flag.Value.Key, flag => bridge.ConfigurationFlagStore.GetValue(game, flag.Value.Key, flag.Value.Type));

            return new JSResponse(request, flags);
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetFlagDefaultValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            string value = request.GetParameter("value");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            dynamic castedValue = bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type);
            switch (flag.Type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    castedValue = bool.Parse(value);
                    break;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    castedValue = int.Parse(value);
                    break;
                case ConfigurationFlagTypes.SELECT_FLAG:
                    castedValue = int.Parse(value);
                    break;
            }
            bridge.ConfigurationFlagStore.SetDefaultValue(flag.Key, castedValue, flag.Type);
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type));
        }
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "key", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetFlagDefaultValue(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IConfigurationFlag flag = bridge.ConfigurationFlags[key];
            return new JSResponse(request, bridge.ConfigurationFlagStore.GetDefaultValue(flag.Key, flag.Type));
        }

        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetFlagDefaultValues(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string key = request.GetParameter("key");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IDictionary<string, dynamic> flags = bridge.ConfigurationFlags.ToDictionary(flag => flag.Value.Key, flag => bridge.ConfigurationFlagStore.GetDefaultValue(flag.Value.Key, flag.Value.Type));
            return new JSResponse(request, flags);
        }
            
        [AjaxMethod(MethodPrefix = "Game")]
        [AjaxMethodParameter(ParameterName = "emulator", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse StartGame(IJSRequest request)
        {
            string emulator = request.GetParameter("emulator");
            string id = request.GetParameter("id");
            IEmulatorBridge bridge = this.CoreInstance.Get<IPluginManager>().Plugins<IEmulatorBridge>()[emulator];
            IGameInfo gameInfo = this.CoreInstance.Get<IGameDatabase>().GetGameByUUID(id);
            var gameStartEvent = new GameStartEventArgs(this.CoreInstance, gameInfo, bridge.EmulatorAssembly, bridge);
            SnowflakeEventManager.EventSource.RaiseEvent(gameStartEvent);
            if (!gameStartEvent.Cancel)
            {
                var instance = gameStartEvent.GameEmulatorBridge.CreateInstance(gameStartEvent.GameInfo);
                this.CoreInstance.Get<IEmulatorInstanceManager>().AddInstance(instance).StartGame();
                }
                return new JSResponse(request, gameInfo);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse HaltRunningGames(IJSRequest request)
        {
            this.CoreInstance.Get<IEmulatorInstanceManager>().ShutdownInstances();
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
