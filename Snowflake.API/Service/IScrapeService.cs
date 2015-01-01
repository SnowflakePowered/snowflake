using System;
using Snowflake.Game;
namespace Snowflake.Service
{
    public interface IScrapeService
    {
       IGameInfo GetGameInfo(string fileName);
    }
}
