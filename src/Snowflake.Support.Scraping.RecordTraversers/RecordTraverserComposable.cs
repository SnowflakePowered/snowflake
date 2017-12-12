using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Support.Scraping.RecordTraversers
{
    public class RecordTraverserComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var stone = serviceContainer.Get<IStoneProvider>();
            var fileTraverser = new FileRecordTraverser();
            var gameTraverser = new GameRecordTraverser(stone, fileTraverser);
            register.RegisterService<ITraverser<IFileRecord>>(fileTraverser);
            register.RegisterService<ITraverser<IGameRecord>>(gameTraverser);
        }
    }
}
