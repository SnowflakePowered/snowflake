using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Extensions;

namespace Snowflake.Service.Manager
{
    public class AjaxManager : IAjaxManager
    {

        private readonly IDictionary<string, IAjaxNamespace> globalNamespace;
        public IReadOnlyDictionary<string, IAjaxNamespace> GlobalNamespace => this.globalNamespace.AsReadOnly();
        public ICoreService CoreInstance { get; }
        public AjaxManager(ICoreService coreInstance)
        {
            this.CoreInstance = coreInstance;
            this.globalNamespace = new Dictionary<string, IAjaxNamespace>();
        }
        public void RegisterNamespace(string namespaceName, IAjaxNamespace namespaceObject)
        {
            if (!this.globalNamespace.ContainsKey(namespaceName))
                this.globalNamespace.Add(namespaceName, namespaceObject);
        }
        public string CallMethod(IJSRequest request)
        {
            var callMethodEvent = new AjaxRequestReceivedEventArgs(this.CoreInstance, request);
            SnowflakeEventManager.EventSource.RaiseEvent(callMethodEvent);
            request = callMethodEvent.ReceivedRequest;
            AjaxResponseSendingEventArgs sendResultEvent;
            try
            {
                IJSResponse result;
                IJSMethod jsMethod = this.GlobalNamespace[request.NameSpace].JavascriptMethods[request.MethodName];
                foreach (AjaxMethodParameterAttribute attr in jsMethod.MethodInfo.GetCustomAttributes<AjaxMethodParameterAttribute>()
                    .Where(attr => attr.Required)
                    .Where(attr => !(request.MethodParameters.Keys.Contains(attr.ParameterName))))
                {
                    result = new JSResponse(request, JSResponse.GetErrorResponse(new JSException(new InvalidOperationException($"{attr.ParameterName}"))), false);
                    sendResultEvent = new AjaxResponseSendingEventArgs(this.CoreInstance, result);
                    SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                    return sendResultEvent.SendingResponse.GetJson();
                }

                result = jsMethod.Method.Invoke(request);
                sendResultEvent = new AjaxResponseSendingEventArgs(this.CoreInstance, result);
                SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
            catch (Exception e)
            {
                var result = new JSResponse(request, JSResponse.GetErrorResponse(new JSException(e, request)), false);
                sendResultEvent = new AjaxResponseSendingEventArgs(this.CoreInstance, result);
                SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
        }
        public async Task<string> CallMethodAsync(IJSRequest request)
        {
            return await Task.Run(() => this.CallMethod(request));
        }


        public void Initialize(IPluginManager pluginManager)
        {
            foreach (var instance in pluginManager.Get<IAjaxNamespace>().Select(ajaxNamespace => ajaxNamespace.Value))
            {
                this.RegisterNamespace(instance.PluginProperties.Get("namespace"), instance);
            }
        }

    }
}
