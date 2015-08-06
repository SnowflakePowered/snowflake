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
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;

namespace Snowflake.Service.Manager
{
    public class AjaxManager : IAjaxManager, ILoadableManager
    {
        public string LoadablesLocation { get; }
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
            var callMethodEvent = new AjaxRequestReceivedEventArgs(CoreService.LoadedCore, request);
            SnowflakeEventManager.EventSource.RaiseEvent<AjaxRequestReceivedEventArgs>(callMethodEvent);
            request = callMethodEvent.ReceivedRequest;
            AjaxResponseSendingEventArgs sendResultEvent;
            try
            {
                IJSResponse result;
                IJSMethod jsMethod = this.GlobalNamespace[request.NameSpace].JavascriptMethods[request.MethodName];
                foreach (AjaxMethodParameterAttribute attr in jsMethod.MethodInfo.GetCustomAttributes<AjaxMethodParameterAttribute>()
                    .Where(attr => attr.Required == true)
                    .Where(attr => !(request.MethodParameters.Keys.Contains(attr.ParameterName))))
                {
                    result = new JSResponse(request, JSResponse.GetErrorResponse($"missing required param {attr.ParameterName}"), false);
                    sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                    SnowflakeEventManager.EventSource.RaiseEvent<AjaxResponseSendingEventArgs>(sendResultEvent);
                    return sendResultEvent.SendingResponse.GetJson();
                }

                result = jsMethod.Method.Invoke(request);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent<AjaxResponseSendingEventArgs>(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
            catch (KeyNotFoundException)
            {
                var result = new JSResponse(request, JSResponse.GetErrorResponse($"method {request.MethodName} not found in namespace {request.NameSpace}"), false);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent<AjaxResponseSendingEventArgs>(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
            catch (Exception e)
            {
                var result = new JSResponse(request, e, false);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent<AjaxResponseSendingEventArgs>(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
        }
        public async Task<string> CallMethodAsync(IJSRequest request)
        {
            return await Task.Run(() => this.CallMethod(request));
        }

        #region ILoadableManager Members

        private void ComposeImports()
        {
            if (!Directory.Exists(Path.Combine(this.LoadablesLocation, "ajax"))) Directory.CreateDirectory(Path.Combine(this.LoadablesLocation, "ajax"));
            var catalog = new DirectoryCatalog(Path.Combine(this.LoadablesLocation, "ajax"));
            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue("coreInstance", CoreService.LoadedCore);
            container.ComposeParts(this);
        }
        public void LoadAll()
        {
            this.ComposeImports();
            foreach (var instance in this.ajaxNamespaces.Select(ajaxNamespace => ajaxNamespace.Value))
            {
                this.RegisterNamespace(instance.PluginInfo["namespace"], instance);
                this.registry.Add(instance.PluginName, typeof(IBaseAjaxNamespace));
            }
        }

        #endregion
    }
}
