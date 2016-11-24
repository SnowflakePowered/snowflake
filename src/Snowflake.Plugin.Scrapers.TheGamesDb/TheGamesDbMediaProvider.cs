using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Snowflake.Scraper.Providers;
using Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi;
using Snowflake.Caching;

namespace Snowflake.Plugin.Scrapers.TheGamesDb
{
    public class TheGamesDbMediaProvider : QueryProvider<IList<IFileRecord>>
    {
        private readonly IKeyedImageCache imageCache;
        public TheGamesDbMediaProvider(IKeyedImageCache imageCache)
        {
            this.imageCache = imageCache;
        }
        public override IEnumerable<IList<IFileRecord>> Query(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public override IList<IFileRecord> QueryBestMatch(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        [Provider]
        [RequiredMetadata("scraper_thegamesdb_id")]
        public IList<IFileRecord> GetImages(IMetadataCollection collection)
        {
            var game = ApiGamesDb.GetGame(Int32.Parse(collection["scraper_thegamesdb_id"]));
            var images = game.Images;
            var boxartFront = this.imageCache.Add(images.BoxartFront.DownloadImage(), collection.Record, ImageTypes.MediaBoxartFront);
            var boxartBack = this.imageCache.Add(images.BoxartBack.DownloadImage(), collection.Record,
                ImageTypes.MediaBoxartBack);
            var banners =
                game.Images.Banners.AsParallel().SelectMany(
                    i => this.imageCache.Add(i.DownloadImage(), collection.Record, ImageTypes.MediaLogo));
            var fanarts = game.Images.Fanart.AsParallel().SelectMany(
                    i => this.imageCache.Add(i.DownloadImage(), collection.Record, ImageTypes.MediaPromotional));
            var screens = game.Images.Screenshots.AsParallel().SelectMany(
                    i => this.imageCache.Add(i.DownloadImage(), collection.Record, ImageTypes.MediaPromotional));

            return Extensions.Concatenate(boxartFront, boxartBack, banners, fanarts, screens).ToList();
        }
    }

    static class Extensions
    {

        //todo make this batch
        public static Image DownloadImage(this ApiGame.ApiGameImages.ApiGameImage image)
        {
            var url = new Uri($@"{ApiGamesDb.BaseImgURL}{image.Path}");
            return Image.FromStream(WebClient.DownloadData(url));
          
        }
        public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] lists)
        {
            return lists.SelectMany(x => x);
        }
    }
}
