using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Snowflake.Utility.Hash;
namespace Snowflake.Romfile
{
    public class ScrapableInfo : IScrapableInfo
    {
        public string QueryableTitle { get; }
        public string GameId { get; }
        public string OriginalFilename { get; }
        public string StonePlatformId { get; }
        public IStructuredFilename StructuredFilename { get; }

        /// <summary>
        /// Initialize a ScrapbleInfo with all details
        /// </summary>
        /// <param name="queryableTitle">Queryable Title</param>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="gameId">Game ID</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string queryableTitle, string originalFilename, string gameId, string stonePlatformId)
        {
            this.QueryableTitle = queryableTitle;
            this.OriginalFilename = originalFilename;
            this.GameId = gameId;
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.StonePlatformId = stonePlatformId;
        }
        /// <summary>
        /// Initialize a ScrapableInfo with parsable queryable title filename
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="gameId">Game ID</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string originalFilename, string gameId, string stonePlatformId)
        {
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.QueryableTitle = this.StructuredFilename.Title;
            this.OriginalFilename = originalFilename;
            this.GameId = gameId;
            this.StonePlatformId = stonePlatformId;
        }

        /// <summary>
        /// Initialize a ScrapableInfo with parsable queryable title filename without a game id
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string originalFilename, string stonePlatformId) : this(originalFilename, null, stonePlatformId)
        {
        }

        public string HashCrc32()
        {
            return FileHash.GetCRC32(this.OriginalFilename);
        }

        public string HashMd5()
        {
            return FileHash.GetMD5(this.OriginalFilename);
        }
    }
}
