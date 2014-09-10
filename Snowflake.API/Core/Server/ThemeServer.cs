using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.IO;
using System.Threading;
using Snowflake.Extensions;
namespace Snowflake.Core.Server
{
    public class ThemeServer : BaseHttpServer
    {
        public string ThemeRoot { get; set; }
        public ThemeServer(string themeRoot) : base(30000)
        {
            this.ThemeRoot = themeRoot;
        }
        
        protected override async Task Process(HttpListenerContext context)
        {
            context.AddAccessControlHeaders();
            string filename = context.Request.Url.AbsolutePath;
            filename = filename.Substring(1);
            if (string.IsNullOrEmpty(filename))
                filename = "index.html";
            filename = Path.Combine(this.ThemeRoot, filename);
            Stream input;
            try
            {
                input = new FileStream(filename, FileMode.Open);
               // context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(Path.GetExtension(filename)));
            }
            catch (FileNotFoundException)
            {
                input = new MemoryStream(new UTF8Encoding().GetBytes("404 not found"));
            }
            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                context.Response.OutputStream.Write(buffer, 0, nbytes);
            input.Close();
            context.Response.OutputStream.Close();
        }
    }
}
