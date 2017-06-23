using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Utility;
using System.IO;

namespace Snowflake.Support.StoreProviders
{
    public class MappedElementCollectionStoreComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        public void Compose(IModule composableModule, IServiceContainer serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            register.RegisterService<IMappedControllerElementCollectionStore>
                            (new SqliteMappedControllerElementCollectionStore(new SqliteDatabase(Path.Combine(appdata.ApplicationData.FullName, "controllermappings.db"))));
        }
    }
}
