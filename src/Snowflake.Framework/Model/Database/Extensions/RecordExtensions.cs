using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Database.Models;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Database.Extensions
{
    internal static class GameRecordExtensions
    {
        public static GameRecordModel AsModel(this IGameRecord @this)
        {
            return new GameRecordModel()
            {
                Platform = @this.PlatformId,
                RecordID = @this.RecordId,
                RecordType = "game",
                Metadata = @this.Metadata.AsModel()
            };
        }

        public static FileRecordModel AsModel(this IFileRecord @this)
        {
            return new FileRecordModel()
            {
                MimeType = @this.MimeType,
                RecordID = @this.RecordId,
                RecordType = "file",
                Metadata = @this.Metadata.AsModel()
            };
        }

        public static FileRecordModel AsModel(this (IFile file, string mimetype) @this)
        {
            return new FileRecordModel()
            {
                MimeType = @this.mimetype,
                RecordID = @this.file.FileGuid,
                RecordType = "file",
                Metadata = new MetadataCollection(@this.file.FileGuid).AsModel()
            };
        }
    }
}
