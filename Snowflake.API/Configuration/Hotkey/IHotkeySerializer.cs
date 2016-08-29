using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;

namespace Snowflake.Configuration.Hotkey
{
    /// <summary>
    /// Serializes a hotkey template.
    /// </summary>
    /// <remarks>
    /// Usually takes in a <see cref="IConfigurationSerializer"/> to use as base serializer
    /// </remarks>
    public interface IHotkeySerializer
    {
        /// <summary>
        /// Serialize the template for a keyboard 
        /// </summary>
        /// <param name="template">The template to serialize</param>
        /// <param name="inputMapping">The input mapping for the target configuration format.</param>
        /// <param name="playerIndex">The player index for this hotkey template, if applicable.</param>
        /// <returns>The serialized configuration section containing hotkey bindings for a keyboard.</returns>
        string SerializeKeyboard(IHotkeyTemplate template, IInputMapping inputMapping, int playerIndex = 0);

        /// <summary>
        /// Serializes the template for a controller
        /// </summary>
        /// <param name="template">The template to serialize</param>
        /// <param name="inputMapping">The input mapping for the target configuration format.</param>
        /// <param name="playerIndex">The player index for this hotkey template, if applicable.</param>
        /// <returns>The serialized configuration section containing hotkey bindings for a controller.</returns>
        string SerializeController(IHotkeyTemplate template, IInputMapping inputMapping,
            int playerIndex = 0);
    }

}
