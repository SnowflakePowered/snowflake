using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    public interface IInputTemplate
    {
        /// <summary>
        /// The controller index of this template instance.
        /// This is zero indexed, Player 1 for example is index 0.
        /// </summary>
        int PlayerIndex { get; set; }

        /// <summary>
        /// The section name for this input template.
        /// May contain an {N} to be replaced with PlayerIndex.
        /// </summary>
        string SectionName { get; }

        /// <summary>
        /// Sets the template to use the appropriate input options for this
        /// set of mapped elements, input device and controller index. 
        /// </summary>
        /// <param name="mappedElements">The set mapped set of elements, from real device to virtual</param>
        /// <param name="inputDevice">The input device used</param>
        /// <param name="playerIndex">The zero-indexed controller position to generate. Player 1 would be index 0</param>
        void SetInputValues(IMappedControllerElementCollection mappedElements, IInputDevice inputDevice, int playerIndex);
        
        /// <summary>
        /// The input options of this input template.
        /// </summary>
        IEnumerable<IInputOption> InputOptions { get; }

        /// <summary>
        /// The options of this configuration section, without key names.
        /// </summary>
        IEnumerable<IConfigurationOption> ConfigurationOptions { get; }

    }
}
