using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
using Snowflake.Ajax;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
		[AjaxMethod(MethodPrefix="GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetBoxartBack(IJSRequest request)
        {
			string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/BoxartBack.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetBoxartFront(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/BoxartFront.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetBoxartFull(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/BoxartFull.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetGameFanart(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/GameFanart.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetGameMusic(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/" + gameCache.GameMusicFileName);

        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetGameVideo(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, gameCache.CacheKey + "/" + gameCache.GameVideoFileName);
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetScreenshots(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            return new JSResponse(request, new GameScreenshotCache(cacheKey).ScreenshotCollection);
        }
    }
}
