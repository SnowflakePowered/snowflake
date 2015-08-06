using System;
using Snowflake.Ajax;
using Snowflake.Game;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
		[AjaxMethod(MethodPrefix="GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetBoxartBack(IJSRequest request)
        {
			string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/BoxartBack.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetBoxartFront(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/BoxartFront.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetBoxartFull(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/BoxartFull.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetGameFanart(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/GameFanart.png");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetGameMusic(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/{gameCache.GameMusicFileName}");

        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "url", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetGameVideo(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            Uri imageUri = new Uri(request.GetParameter("url"));
            IGameMediaCache gameCache = new GameMediaCache(cacheKey);
            gameCache.SetBoxartBack(imageUri);
            return new JSResponse(request, $"{gameCache.CacheKey}/{gameCache.GameVideoFileName}");
        }
        [AjaxMethod(MethodPrefix = "GameCache")]
        [AjaxMethodParameter(ParameterName = "id", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetScreenshots(IJSRequest request)
        {
            string cacheKey = request.GetParameter("id");
            return new JSResponse(request, new GameScreenshotCache(cacheKey).ScreenshotCollection);
        }
    }
}
