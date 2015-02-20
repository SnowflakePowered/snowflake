using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Ajax;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System;
using System.Linq;
using Snowflake.Extensions;

namespace Snowflake.Service.Manager
{
    public class AjaxManager : IAjaxManager, ILoadableManager
    {
        public string LoadablesLocation { get; private set; }
        private IDictionary<string, Type> registry;
        public IReadOnlyDictionary<string, Type> Registry { get { return this.registry.AsReadOnly(); } }
        [ImportMany(typeof(IBaseAjaxNamespace))]
        IEnumerable<Lazy<IBaseAjaxNamespace>> ajaxNamespaces;
        private IDictionary<string, IBaseAjaxNamespace> globalNamespace;
        public IReadOnlyDictionary<string, IBaseAjaxNamespace> GlobalNamespace { get { return this.globalNamespace.AsReadOnly(); } }
        public AjaxManager(string loadablesLocation)
        {
            this.globalNamespace = new Dictionary<string, IBaseAjaxNamespace>();
            this.LoadablesLocation = loadablesLocation;
            this.registry = new Dictionary<string, Type>();
        }
        public void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject)
        {
            if (!this.globalNamespace.ContainsKey(namespaceName))
                this.globalNamespace.Add(namespaceName, namespaceObject);
        }
        public string CallMethod(IJSRequest request)
        {
            try
            {
                IJSResponse result = this.GlobalNamespace[request.NameSpace].JavascriptMethods[request.MethodName].Method.Invoke(request);
                return result.GetJson();
            }
            catch (Exception e)
            {
                return new JSResponse(request, e, false).GetJson();
            }
        }
        public async Task<string> CallMethodAsync(IJSRequest request)
        {
            return await Task.Run(() => this.CallMethod(request));
        }

        #region ILoadableManager Members

        private void ComposeImports()
        {
            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "ajax"));
            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue("coreInstance", CoreService.LoadedCore);
            container.ComposeParts(this);
        }
        public void LoadAll()
        {
            this.ComposeImports();
            foreach (var ajaxNamespace in this.ajaxNamespaces)
            {
                var instance = ajaxNamespace.Value;
                this.RegisterNamespace(instance.PluginInfo["namespace"], instance);
                this.registry.Add(instance.PluginName, typeof(IBaseAjaxNamespace));
            }
        }
        #endregion
    }
}
