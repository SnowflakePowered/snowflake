using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator;

namespace Snowflake.Service.Manager
{
    public interface IEmulatorBridgeManager : IDisposable
    {
        /// <summary>
        /// Adds an instance to the container, and returns a unique ID for the instance
        /// </summary>
        void AddInstance(IEmulatorInstance instance);
        /// <summary>
        /// Removes an instance from the container;
        /// </summary>
        /// <param name="instance"></param>
        void RemoveInstance(IEmulatorInstance instance);
        /// <summary>
        /// Gets an instance given it's instance ID
        /// </summary>
        /// <param name="instanceId"></param>
        void GetInstance(string instanceId);
        /// <summary>
        /// Gets all instances that belong to a certain emulator
        /// </summary>
        /// <param name="bridge">The emulator bridge to get</param>
        void GetInstances(IEmulatorBridge bridge);
        /// <summary>
        /// Get instances of a certain state.
        /// </summary>
        /// <param name="state">The state of the instances</param>
        void GetInstances(EmulatorInstanceState state);
        /// <summary>
        /// Shuts down every single instance in the container, and purges the container.
        /// </summary>
        void ShutdownInstances();
        /// <summary>
        /// Removes hanging instances (ones that have shut down) from the container
        /// </summary>
        void PurgeHangingInstances();
        /// <summary>
        /// Disposes the container, shutting down all hanging instances.
        /// </summary>
        void Dispose();
    }
}
