using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Game;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetGameResults(IJSRequest request)
        {
            return new JSResponse(request, null);
        }
        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetGameInfo(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]

        public IJSResponse AddGameInfo(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]

        public IJSResponse GetGame(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse GetGamesForPlatform(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]

        public IJSResponse GetFlags(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse SetFlag(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse StartGame(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse HaltRunningGames(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse SearchGames(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Game")]
        public IJSResponse SearchGamesInPlatform(IJSRequest request)
        {
            return new JSResponse(request, null);
        }
    }
}
