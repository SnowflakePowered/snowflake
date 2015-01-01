using System;
using Snowflake.Information;
namespace Snowflake.Game
{
    public interface IGameInfo : IInfo
    {
        string CRC32 { get; }
        string FileName { get; }
        string UUID { get; }
    }
}
