using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Game
{
    public class GameDatabaseContext : DbContext
    {
        public DbSet<IGameInfo> GamesTable { get; set; }
    }
}
