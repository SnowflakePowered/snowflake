using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.Framework.Exceptions;
using Snowflake.Records.Metadata;
using Snowflake.Records.File;
using Snowflake.Scraper;

namespace Snowflake.Support.Remoting.Resources
{
    public class Games
    {

        private IGameLibrary Library { get; }
        private IStoneProvider Stone { get; }
        private List<string> ForbiddenDeleteKeys = new List<string> { "game_title", "game_platform", "file_linkedrecord" };
        public Games(IStoneProvider stone, IGameLibrary library)
        {
            this.Library = library;
            this.Stone = stone;
        }

        
        [Endpoint(RequestVerb.Read, "~:games")]
        public IEnumerable<IGameRecord> ListGames()
        {
            return this.Library.GetAllRecords();
        }

        [Endpoint(RequestVerb.Read, "~:games:{guid}")]
        public IGameRecord GetGame(Guid guid) 
        {
            var game = this.Library.Get(guid);
            if(game == null) throw new RecordNotFoundException(guid);
            return game;
        }

        public IGameRecord SetMetadata(Guid game, string metadataKey, string value)
        {
            this.Library.MetadataLibrary.Set(new RecordMetadata(metadataKey, value, game));
            try
            {
                return this.Library.Get(game);
            }
            catch (Exception)
            {
                throw new RecordNotFoundException(game);
            }
        }

        public IGameRecord DeleteMetadata(Guid game, string metadataKey)
        {
            if (this.ForbiddenDeleteKeys.Contains(metadataKey)) throw new ProtectedMetadataException(metadataKey);
            try
            {
                this.Library.MetadataLibrary.Remove((this.Library.Get(game).Metadata as IDictionary<string, IRecordMetadata>)[metadataKey]);

            }catch(KeyNotFoundException){
                throw new MetadataNotFoundException(game, metadataKey);
            }
            return this.Library.Get(game);
        }

        public IEnumerable<IGameRecord> DeleteGame(Guid guid)
        {
            this.Library.Remove(guid);
            return this.Library.GetAllRecords();
        }

        public IGameRecord CreateGame(string title, string platformId)
        {
            try
            {
                var platform = this.Stone.Platforms[platformId];
                var record = new GameRecord(platform, title);
                this.Library.Set(record);
                return this.Library.Get(record.Guid);
            }catch(KeyNotFoundException)
            {
                throw new UnknownPlatformException(platformId);
            }
        }

        public IGameRecord CreateFile(Guid gameGuid, string path, string mimetype)
        {
            if (path == null || mimetype == null) throw new InvalidFileException();
            var file = new FileRecord(path, mimetype, gameGuid);
            this.Library.FileLibrary.Set(file);
            return this.Library.Get(gameGuid);
        }

        public IGameRecord DeleteFile(Guid gameGuid, Guid fileGuid)
        {
            this.Library.FileLibrary.Remove(fileGuid);
            return this.Library.Get(gameGuid);
        }

        public IGameRecord Scrape(string filePath)
        {
            return null;
        }

        public IGameRecord DeleteFileMetadata(Guid gameGuid, Guid fileGuid, string metadataKey)
        {
            if (this.ForbiddenDeleteKeys.Contains(metadataKey)) throw new ProtectedMetadataException(metadataKey);
            try
            {
                this.Library.MetadataLibrary.Remove((this.Library.FileLibrary.Get(fileGuid).Metadata as IDictionary<string, IRecordMetadata>)[metadataKey]);

            }
            catch (KeyNotFoundException)
            {
                throw new MetadataNotFoundException(fileGuid, metadataKey);
            }
            return this.Library.Get(gameGuid);
        }


        public IGameRecord SetFileMetadata(Guid game, Guid fileGuid, string metadataKey, string value)
        {
            this.Library.MetadataLibrary.Set(new RecordMetadata(metadataKey, value, fileGuid));
            try
            {
                return this.Library.Get(game);
            }
            catch (Exception)
            {
                throw new RecordNotFoundException(game);
            }
        }
    }
}
