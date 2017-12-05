using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Romfile.FileSignatures.Composers
{
    public class FileSignatureMatcherComposer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
            var stoneProvider = serviceContainer.Get<IStoneProvider>();
            serviceContainer.Get<IServiceRegistrationProvider>().
                RegisterService<IFileSignatureMatcher>(new FileSignatureMatcher(stoneProvider));
        }
    }
}
