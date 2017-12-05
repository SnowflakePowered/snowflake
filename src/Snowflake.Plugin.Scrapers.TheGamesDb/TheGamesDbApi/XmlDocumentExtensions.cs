using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Snowflake.Utility;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    internal static class XmlDocumentExtensions
    {
        internal static void Load(this XmlDocument @this, string url)
        {
            var data = WebClient.DownloadData(new Uri(url));
            @this.Load(data);
        }
    }
}
