using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.FileSignatures
{
    public class FileSignaturesContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var pluginManager = coreInstance.Get<IPluginManager>();

            pluginManager.Register<IFileSignature>(new NintendoN64FileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBCFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBAFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGCNISOFileSignature(coreInstance));

        
            pluginManager.Register<IFileSignature>(new NintendoNDSFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoNESFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoSNESFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoWiiFileSignature(coreInstance));

            pluginManager.Register<IFileSignature>(new Sega32XFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaCDFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaDCFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaGENFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaGGFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaSATFileSignature(coreInstance));

            pluginManager.Register<IFileSignature>(new SonyPS2ISOFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SonyPSPISOFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SonyPSXISOFileSignature(coreInstance));





        }
    }
}
