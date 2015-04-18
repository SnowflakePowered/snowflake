using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using Snowflake.Extensions;
using Snowflake.Game;
using Mono.Net;

namespace Snowflake.Service.HttpServer
{
    public class GameCacheServer : BaseHttpServer
    {

        public GameCacheServer()
            : base(30002)
        {

        }

        protected override async Task Process(HttpListenerContext context)
        {
            context.AddAccessControlHeaders();
            string getRequest = context.Request.Url.AbsolutePath.Remove(0, 1); //Remove first slash
            Stream input = null;
            try
            {
                string gameUUID = HttpUtility.UrlDecode(getRequest.Split('/')[0]);
                string fileName = HttpUtility.UrlDecode(getRequest.Split('/')[1]);
                if (fileName.StartsWith("BoxartFront"))
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    input = new FileStream(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, "BoxartFront.png"), FileMode.Open);
                }
                if (fileName.StartsWith("BoxartBack"))
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    input = new FileStream(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, "BoxartBack.png"), FileMode.Open);
                }
                if (fileName.StartsWith("BoxartFull"))
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    input = new FileStream(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, "BoxartFull.png"), FileMode.Open);
                }
                if (fileName.StartsWith("GameFanart"))
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    input = new FileStream(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, "GameFanart.png"), FileMode.Open);

                }
                if (fileName.StartsWith("GameMusic"))
                {
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    string _fileName = mediaCache.GameMusicFileName;
                    context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(_fileName));
                    string filePath = Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, _fileName);
                    input = new FileStream(filePath, FileMode.Open);
                }
                if (fileName.StartsWith("GameVideo"))
                {
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    string _fileName = mediaCache.GameVideoFileName;
                    context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(_fileName));
                    string filePath = Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, _fileName);
                    input = new FileStream(filePath, FileMode.Open);
                }
                if (fileName.StartsWith("screenshot"))
                {
                    context.Response.AddHeader("Content-Type", "image/png");
                    int index;
                    Int32.TryParse(fileName.Split('~')[1], out index);
                    IGameScreenshotCache screenCache = new GameScreenshotCache(gameUUID);
                    string _fileName = screenCache.ScreenshotCollection[index];
                    string filePath = Path.Combine(screenCache.RootPath, screenCache.CacheKey, _fileName);
                    input = new FileStream(filePath, FileMode.Open);

                }
            }
            catch (FileNotFoundException)
            {
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 not found"));
            }
            catch
            {
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 not found"));
            }
            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            await Task.Run(() =>
            {
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
            });
            input.Close();
            context.Response.OutputStream.Close();
        }
    }
}
