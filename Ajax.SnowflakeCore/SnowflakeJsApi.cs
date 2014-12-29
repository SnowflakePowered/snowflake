using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Core;
using System.ComponentModel.Composition;
namespace Ajax.SnowflakeCore
{
    public class SnowflakeJsApi : BaseAjaxNamespace
    {
        public SnowflakeJsApi([Import("coreInstance")] FrontendCore coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            
        }

        [AjaxMethod(MethodPrefix = "Get", MethodName = "NotTest")]
        public JSResponse Test(JSRequest request)
        {
            return new JSResponse(request, "success from Api");
        }

        [AjaxMethod]
        public JSResponse GetTest(JSRequest request)
        {
            var game = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
            return  new JSResponse(request, game);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public JSResponse GetAllPlatforms(JSRequest request)
        {
            return new JSResponse(request, FrontendCore.LoadedCore.LoadedPlatforms);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public JSResponse GetPlatform(JSRequest request)
        {
            var platform = request.GetParameter("platform");
            if (platform == null || !FrontendCore.LoadedCore.LoadedPlatforms.ContainsKey(platform)) return new JSResponse(request, null);
            return new JSResponse(request, FrontendCore.LoadedCore.LoadedPlatforms[platform]);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGamesByPlatform(JSRequest request)
        {
            var platform = request.GetParameter("platform");
            if (platform == null || !FrontendCore.LoadedCore.LoadedPlatforms.ContainsKey(platform)) return new JSResponse(request, null);
            return new JSResponse(request, FrontendCore.LoadedCore.GameDatabase.GetGamesByPlatform(platform));
        }
        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGameByUuid(JSRequest request)
        {
            var uuid = request.GetParameter("uuid");
            return uuid != null ? new JSResponse(request, FrontendCore.LoadedCore.GameDatabase.GetGameByUUID(uuid)) : new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGamesByName(JSRequest request)
        {
            var name = request.GetParameter("name");
            return name != null ? new JSResponse(request, FrontendCore.LoadedCore.GameDatabase.GetGamesByName(name)) : new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetAllGames(JSRequest request)
        {
            return new JSResponse(request, FrontendCore.LoadedCore.GameDatabase.GetAllGames());
        }
        [AjaxMethod(MethodPrefix = "Command")]
        public JSResponse RunGameByUuid(JSRequest request)
        {
            try
            {
                var uuid = request.GetParameter("uuid");
                var game = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID(uuid);
                var platform = FrontendCore.LoadedCore.LoadedPlatforms[game.PlatformId];
                FrontendCore.LoadedCore.PluginManager.LoadedEmulators[platform.Defaults.Emulator].StartRom(game);
                return new JSResponse(request, "success"); //todo replace with proper success response
            }
            catch
            {
                return new JSResponse(request, null);
            }
   
        }
        [AjaxMethod(MethodPrefix = "Command")]
        public JSResponse Quit(JSRequest request)
        {
            //todo call an exit event here
            return new JSResponse(request, null);
        }
    }
}
