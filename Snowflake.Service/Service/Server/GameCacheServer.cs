using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
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
                    if (context.Request.QueryString["scale"] != null)
                    {
                        string _scalePercentage = context.Request.QueryString["scale"];
                        int scalePercentage;
                        if (Int32.TryParse(_scalePercentage, out scalePercentage))
                        {
                            if (scalePercentage <= 100 && scalePercentage > 0)
                            {
                                double trueScalePercentage = scalePercentage / 100D;
                                using (Image initialImage = Image.FromFile(Path.Combine(filePath)))
                                {
                                    using (Image resizedImage = GameMediaCache.ResizeImage(initialImage, trueScalePercentage))
                                    {
                                        using (MemoryStream _input = new MemoryStream())
                                        {
                                            resizedImage.Save(_input, ImageFormat.Png);
                                            input = new MemoryStream(_input.ToArray());
                                        }
                                    }
                                }

                            }

                        }
                    }
                    else
                    {
                        input = new FileStream(filePath, FileMode.Open);
                    }
                }
                if (fileName.StartsWith("BoxartBack") || fileName.StartsWith("BoxartFront") || fileName.StartsWith("BoxartFull") || fileName.StartsWith("GameFanart"))
                {
                    StringBuilder imageFileName = new StringBuilder();
                    if (fileName.StartsWith("BoxartBack"))
                    {
                        imageFileName.Append("BoxartBack.png");
                    }
                    else if (fileName.StartsWith("BoxartFront"))
                    {
                        imageFileName.Append("BoxartFront.png");
                    }
                    else if (fileName.StartsWith("BoxartFull"))
                    {
                        imageFileName.Append("BoxartFull.png");
                    }
                    else if (fileName.StartsWith("GameFanart"))
                    {
                        imageFileName.Append("GameFanart.png");
                    }
                    
                    if (fileName.Contains("@10"))
                    {
                        imageFileName.Insert(0, "@10_");
                    }
                    else if (fileName.Contains("@25"))
                    {
                        imageFileName.Insert(0, "@25_");
                    }
                    else if (fileName.Contains("@50"))
                    {
                        imageFileName.Insert(0, "@50_");
                    }
                    else if (fileName.Contains("@75_"))
                    {
                        imageFileName.Insert(0, "@75_");
                    }

                    context.Response.AddHeader("Content-Type", "image/png");
                    IGameMediaCache mediaCache = new GameMediaCache(gameUUID);
                    if (context.Request.QueryString["scale"] != null)
                    {
                        string _scalePercentage = context.Request.QueryString["scale"];
                        int scalePercentage;
                        if (Int32.TryParse(_scalePercentage, out scalePercentage))
                        {
                            if (scalePercentage <= 100 && scalePercentage > 0){
                                double trueScalePercentage = scalePercentage / 100D ;
                                using (Image initialImage = Image.FromFile(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, imageFileName.ToString())))
                                {
                                  using (Image resizedImage = GameMediaCache.ResizeImage(initialImage, trueScalePercentage))
                                  {
                                      using (MemoryStream _input = new MemoryStream())
                                      {
                                          resizedImage.Save(_input, ImageFormat.Png);
                                          input = new MemoryStream(_input.ToArray());
                                      }
                                  }
                                }

                            }
                            
                        }
                    }
                    else 
                    {
                       input = new FileStream(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey, imageFileName.ToString()), FileMode.Open);
                    }
                    
                }
            }
            catch (FileNotFoundException)
            {
                context.Response.AddHeader("Content-Type", "text/plain");
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "404 Not Found";
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 Not Found"));
            }
            catch (IndexOutOfRangeException)
            {
                context.Response.AddHeader("Content-Type", "text/plain");
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "404 Not Found";
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 Not Found"));
            }
            catch
            {
                context.Response.AddHeader("Content-Type", "text/plain");
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "404 Not Found";
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 Not Found"));
            }
            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            await Task.Run(() =>
            {
                if (input == null) //not a null check but a conditional
                {
                    context.Response.AddHeader("Content-Type", "text/plain");
                    context.Response.StatusCode = 404;
                    context.Response.StatusDescription = "404 Not Found";
                    input = new MemoryStream(new UTF8Encoding().GetBytes("404 Not Found"));
                }
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
            });
            input.Close();
            context.Response.OutputStream.Close();
        }
    }
}
