using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Hotkey;
using Snowflake.Input.Controller;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys;
using Snowflake.Utility;
using Xunit;
namespace Snowflake.Configuration.Tests
{
    
    public class HotkeyTemplateStoreTests
    {
        [Fact]
        public void GetUnsetTemplateTest()
        {
            IHotkeyTemplateStore store = new SqliteHotkeyTemplateStore(new SqliteDatabase(Path.GetTempFileName()));
            var template = store.GetTemplate<RetroarchHotkeyTemplate>();
            var newtemplate = new RetroarchHotkeyTemplate();
            
            foreach (var option in template.HotkeyOptions)
            {
                Assert.Equal(option.Value, newtemplate.HotkeyOptions.First(k => k.KeyName == option.KeyName).Value); 
            }
        }

        [Fact]
        public void RetrieveTemplateTest()
        {
            IHotkeyTemplateStore store = new SqliteHotkeyTemplateStore(new SqliteDatabase(Path.GetTempFileName()));
            var template = new RetroarchHotkeyTemplate();
            template.InputAudioMute = new HotkeyTrigger(template.InputAudioMute.KeyboardTrigger, ControllerElement.Button0);
            store.SetTemplate(template);
            var retTemplate = store.GetTemplate<RetroarchHotkeyTemplate>();

            foreach (var option in retTemplate.HotkeyOptions)
            {
                Assert.Equal(option.Value, retTemplate.HotkeyOptions.First(k => k.KeyName == option.KeyName).Value);
            }

            //replace

            template.InputCheatIndexMinus = new HotkeyTrigger(KeyboardKey.Key1, ControllerElement.AxisLeftAnalogNegativeX);
            store.SetTemplate(template);
            retTemplate = store.GetTemplate<RetroarchHotkeyTemplate>();
            foreach (var option in retTemplate.HotkeyOptions)
            {
                Assert.Equal(option.Value, retTemplate.HotkeyOptions.First(k => k.KeyName == option.KeyName).Value);
            }
        }
    }
}
