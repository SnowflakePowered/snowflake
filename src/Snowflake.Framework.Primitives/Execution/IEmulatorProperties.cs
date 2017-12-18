using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Platform;

namespace Snowflake.Execution
{
    public interface IEmulatorProperties
    {
        /// <summary>
        /// Gets the save type this emulator uses. Emulators with the same save type will share a directory
        /// allowing save sharing across emulators. Should your emulator support a different save type,
        /// please change this to a unique value to avoid save conflicts.
        /// </summary>
        string SaveFormat { get; }

        /// <summary>
        /// Gets a list of mimetypes this emulator can execute. This is a required key,
        /// an emulator without supported mimetypes will not display for any
        /// file type.
        /// </summary>
        IEnumerable<string> Mimetypes { get; }

        /// <summary>
        /// Gets a list of BIOS files this emulator requires, listed under the optional metadata key
        /// requiredbios.
        /// </summary>
        IEnumerable<IBiosFile> RequiredSystemFiles { get; }

        /// <summary>
        /// Gets a list of BIOS files this emulator requires, listed under the optional metadata key
        /// optionalbios.
        /// </summary>
        IEnumerable<IBiosFile> OptionalSystemFiles { get; }

        /// <summary>
        /// Gets a list of emulator special capabiities strings under the capabilities metadata key.
        /// This key is optional, and there is no formal specification for these capabilities,
        /// which may include ingame overlays, cloud saves, etc. It is used to indicate to clients
        /// that your emulator supports these capabilities upon a formally agreed upon API.
        /// </summary>
        IEnumerable<string> SpecialCapabilities { get; }
    }
}
