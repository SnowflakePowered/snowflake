using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using Snowflake.Extensions;
using System.Web;
using System.IO;
using Snowflake.Information.MediaStore;

namespace Snowflake.Service.HttpServer
{
    public class FileMediaStoreServer : BaseHttpServer
    {
        public string MediaStoreRoot { get; set; }

        public FileMediaStoreServer(string mediaStoreRoot) : base(30002)
        {
            this.MediaStoreRoot = mediaStoreRoot;
        }
        protected override async Task Process(HttpListenerContext context)
        {
            context.AddAccessControlHeaders();
            string getRequest = context.Request.Url.AbsolutePath.Remove(0, 1); //Remove first slash

            Stream input = null;
            try
            {
                var mediastoreKey = HttpUtility.UrlDecode(getRequest.Split('/')[0]);
                var mediastoreSection = getRequest.Split('/')[1];
                var mediastoreItem = getRequest.Split('/')[2];
                var mediaStore = new FileMediaStore(mediastoreKey);
                switch(mediastoreSection){
                    case "images":
                       input = new FileStream(Path.Combine(this.MediaStoreRoot, mediastoreKey, mediastoreSection, mediaStore.Images[mediastoreItem]), FileMode.Open);
                       context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(mediaStore.Images[mediastoreItem]));

                       break;
                   case "audio":
                       input = new FileStream(Path.Combine(this.MediaStoreRoot, mediastoreKey, mediastoreSection, mediaStore.Audio[mediastoreItem]), FileMode.Open);
                       context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(mediaStore.Audio[mediastoreItem]));

                       break;
                   case "video":
                       input = new FileStream(Path.Combine(this.MediaStoreRoot, mediastoreKey, mediastoreSection, mediaStore.Video[mediastoreItem]), FileMode.Open);
                       context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(mediaStore.Video[mediastoreItem]));
                       break;
                    case "resoruces":
                       input = new FileStream(Path.Combine(this.MediaStoreRoot, mediastoreKey, mediastoreSection, mediaStore.Resources[mediastoreItem]), FileMode.Open);
                       context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(mediaStore.Resources[mediastoreItem]));
                       break;
                }
            }
            catch
            {
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 not found"));
            }
           
            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            //await Task.Run(() =>
        //    {
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
        //    });
            input.Close();
            context.Response.OutputStream.Close();
        }
    }
}
