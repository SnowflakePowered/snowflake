using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    public interface ISaveGameManager
    {
        Task<ISaveGame> CreateSave(string type, Func<IDirectory, Task> factory);
        Task<ISaveGame> CreateSave(string type, IEnumerable<string> tags, Func<IDirectory, Task> factory);
        ISaveGame? GetLatestSave(string type);
        ISaveGame? GetSave(Guid guid);
        IEnumerable<ISaveGame> GetSaves();
    }
}