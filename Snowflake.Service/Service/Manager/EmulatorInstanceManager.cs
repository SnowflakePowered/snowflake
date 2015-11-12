using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator;

namespace Snowflake.Service.Manager
{
    public class EmulatorInstanceManager : IEmulatorInstanceManager
    {
        private readonly ICoreService coreService;
        private readonly IList<IEmulatorInstance> emulatorInstances;
        public EmulatorInstanceManager(ICoreService coreService)
        {
            this.coreService = coreService;
            this.emulatorInstances = new List<IEmulatorInstance>();
        }
        public void AddInstance(IEmulatorInstance instance)
        {
            this.emulatorInstances.Add(instance);
        }

        public void RemoveInstance(IEmulatorInstance instance)
        {
            this.emulatorInstances.Remove(instance);
        }

        public IEmulatorInstance GetInstance(string instanceId)
        {
            return this.emulatorInstances.First(i => i.InstanceId == instanceId);
        }

        public IEnumerable<IEmulatorInstance> GetInstances(IEmulatorBridge bridge)
        {
            return this.emulatorInstances.Where(i => i.InstanceEmulator == bridge);
        }

        public IEnumerable<IEmulatorInstance> GetInstances(EmulatorInstanceState state)
        {
            return this.emulatorInstances.Where(i => i.InstanceState == state);
        }

        public void CloseInstances()
        {
            foreach (var instance in this.emulatorInstances)
            {
                instance.CleanupInstance();
            }
        }

        public void PurgeHangingInstances()
        {
            foreach(var instance in this.emulatorInstances.Where(i => i.InstanceState == EmulatorInstanceState.InstanceClosed))
            {
                this.emulatorInstances.Remove(instance);
            }
        }

        public void Dispose()
        {
            this.PurgeHangingInstances();
        }
    }
}
