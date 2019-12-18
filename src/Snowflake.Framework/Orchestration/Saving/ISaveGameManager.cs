using Snowflake.Filesystem;
using System;
using System.Collections.Generic;

namespace Snowflake.Orchestration.Saving
{
    public interface ISaveGameManager
    {
        ISaveGame CreateSave(string type, Action<IDirectory> factory);
        ISaveGame CreateSave(string type, IEnumerable<string> tags, Action<IDirectory> factory);
        ISaveGame? GetLatestSave(string type);
        ISaveGame? GetSave(Guid guid);
        IEnumerable<ISaveGame> GetSaves();
    }
}