using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Romfile.FileSignatures.Composers
{
    public class FileSignatureMatcherComposer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceContainer serviceContainer)
        {
            var stoneProvider = serviceContainer.Get<IStoneProvider>();
            serviceContainer.Get<IServiceRegistrationProvider>().
                RegisterService<IFileSignatureMatcher>(new FileSignatureMatcher(stoneProvider));
        }
    }
}
