using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.StoreProviders
{
    public class MappedElementCollectionStoreComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ISqliteDatabaseProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqlDb = serviceContainer.Get<ISqliteDatabaseProvider>();
            register.RegisterService<IMappedControllerElementCollectionStore>
                            (new SqliteMappedControllerElementCollectionStore(sqlDb.CreateDatabase("controllermappings")));
        }
    }
}
