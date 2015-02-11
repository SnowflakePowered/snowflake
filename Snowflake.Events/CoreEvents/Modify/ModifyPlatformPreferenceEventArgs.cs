using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyPlatformPreferenceEventArgs : SnowflakeEventArgs
    {
        public IPlatformInfo Platform { get; private set; }
        public string PreviousPreference { get; private set; }
        public string ModifiedPreference { get; set; }
        public PreferenceType PreferenceType { get; private set; }
        public ModifyPlatformPreferenceEventArgs(ICoreService eventCoreInstance, string previousPreference, string modifiedPreference, IPlatformInfo platform, PreferenceType preferenceType)
            : base(eventCoreInstance)
        {
            this.Platform = platform;
            this.PreferenceType = preferenceType;
            this.PreviousPreference = previousPreference;
            this.ModifiedPreference = modifiedPreference;
        }
    }
    public enum PreferenceType
    {
        PREF_IDENTIFIER,
        PREF_EMULATOR,
        PREF_SCRAPER
    }
}
