using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Core.Manager.Interface;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace Snowflake.Core.Manager
{
    public class AjaxManager : IAjaxManager, ILoadableManager
    {
        public IDictionary<string, IBaseAjaxNamespace> GlobalNamespace { get; private set; }
        
        public AjaxManager(string loadablesLocation)
        {
            this.GlobalNamespace = new Dictionary<string, IBaseAjaxNamespace>();
            this.LoadablesLocation = loadablesLocation;
            this.Registry = new Dictionary<string, Type>();
            this.ComposeImports();

        }
        public void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject)
        {
            if (!this.GlobalNamespace.ContainsKey(namespaceName))
                this.GlobalNamespace.Add(namespaceName, namespaceObject);
        }
        public string CallMethod(JSRequest request)
        {
            try
            {
                JSResponse result = this.GlobalNamespace[request.NameSpace].JavascriptMethods[request.MethodName].Invoke(request);
                return result.GetJson();
            }
            catch (KeyNotFoundException)
            {
                return JsonConvert.Undefined;
            }
        }
        public async Task<string> CallMethodAsync(JSRequest request)
        {
            return await Task.Run(() => this.CallMethod(request));
        }

        #region ILoadableManager Members
        public string LoadablesLocation { get; private set; }
        public IDictionary<string, Type> Registry { get; private set; }
        [ImportMany(typeof(IBaseAjaxNamespace))]
        IEnumerable<Lazy<IBaseAjaxNamespace>> ajaxNamespaces;
        private void ComposeImports()
        {
            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "ajax"));
            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }
        public void LoadAll()
        {
            foreach (var instance in this.ajaxNamespaces.Select(plugin => plugin.Value))
            {
                this.RegisterNamespace(instance.PluginInfo["namespace"], instance);
                this.Registry.Add(instance.PluginName, typeof(IBaseAjaxNamespace));
            }
        }
        #endregion
    }
}
