using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Service;
using System.ComponentModel.Composition;
namespace Ajax.SnowflakeCore
{
    public class SnowflakeJsApi : BaseAjaxNamespace
    {
        public SnowflakeJsApi([Import("coreInstance")] CoreService coreInstance)
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
            var game = CoreService.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
            return  new JSResponse(request, game);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public JSResponse GetAllPlatforms(JSRequest request)
        {
            return new JSResponse(request, CoreService.LoadedCore.LoadedPlatforms);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public JSResponse GetPlatform(JSRequest request)
        {
            var platform = request.GetParameter("platform");
            if (platform == null || !CoreService.LoadedCore.LoadedPlatforms.ContainsKey(platform)) return new JSResponse(request, null);
            return new JSResponse(request, CoreService.LoadedCore.LoadedPlatforms[platform]);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGamesByPlatform(JSRequest request)
        {
            var platform = request.GetParameter("platform");
            if (platform == null || !CoreService.LoadedCore.LoadedPlatforms.ContainsKey(platform)) return new JSResponse(request, null);
            return new JSResponse(request, CoreService.LoadedCore.GameDatabase.GetGamesByPlatform(platform));
        }
        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGameByUuid(JSRequest request)
        {
            var uuid = request.GetParameter("uuid");
            return uuid != null ? new JSResponse(request, CoreService.LoadedCore.GameDatabase.GetGameByUUID(uuid)) : new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetGamesByName(JSRequest request)
        {
            var name = request.GetParameter("name");
            return name != null ? new JSResponse(request, CoreService.LoadedCore.GameDatabase.GetGamesByName(name)) : new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public JSResponse GetAllGames(JSRequest request)
        {
            return new JSResponse(request, CoreService.LoadedCore.GameDatabase.GetAllGames());
        }
        [AjaxMethod(MethodPrefix = "Command")]
        public JSResponse RunGameByUuid(JSRequest request)
        {
            try
            {
                var uuid = request.GetParameter("uuid");
                var game = CoreService.LoadedCore.GameDatabase.GetGameByUUID(uuid);
                var platform = CoreService.LoadedCore.LoadedPlatforms[game.PlatformId];
                CoreService.LoadedCore.PluginManager.LoadedEmulators[platform.Defaults.Emulator].StartRom(game);
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
