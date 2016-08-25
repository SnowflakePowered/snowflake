using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Utility;
using Dapper;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
    public class SqliteHotkeyTemplateStore : IHotkeyTemplateStore
    {
        private readonly SqliteDatabase backingDatabase;

        public SqliteHotkeyTemplateStore(SqliteDatabase sqliteDatabase)
        {
            this.backingDatabase = sqliteDatabase;
            this.CreateDatabase();
            
        }

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("hotkeys",
                               "TemplateFilename TEXT",
                               "SectionName TEXT",
                               "TemplateType TEXT",
                               "OptionKey TEXT",
                               "OptionValue TEXT",
                               "PRIMARY KEY (TemplateFilename, SectionName, TemplateType, OptionKey)");
        }

        public T GetTemplate<T>(string templateFilename, HotkeyTemplateType templateType)
            where T : IHotkeyTemplate, new()
        {
            var values = this.GetValues(templateFilename, templateType);
            return values.Any() ? this.BuildHotkeyTemplate<T>(values) : new T();
        }

        public T GetTemplate<T>() where T : IHotkeyTemplate, new()
        {
            T template = new T();
            return this.GetTemplate<T>(template.FileName, template.TemplateType);
        }

        public void SetTemplate(IHotkeyTemplate template)
        {
            throw new NotImplementedException();
        }

     
        private IDictionary<string, string> GetValues(string templateFilename, HotkeyTemplateType templateType)
        {
            return this.backingDatabase.Query<IDictionary<string, string>>(dbConnection =>
            {
                var retValues = new Dictionary<string, string>();
                var values = dbConnection.Query<HotkeyTemplateRecord>
                ("SELECT * FROM hotkeys WHERE TemplateFilename = @TemplateFilename AND TemplateType = @TemplateType",
                    new { TemplateFilename = templateFilename, TemplateType = templateType.ToString() });
                foreach (var options in values)
                {
                    retValues[options.OptionKey] = options.OptionValue;
                }
                return retValues;
            });
        }

        /// <summary>
        /// Builds a configuration collection from the ground up using a set of keyed values.
        /// </summary>
        /// <typeparam name="T">The type to build</typeparam>
        /// <param name="values">The values</param>
        /// <returns>The configuration collection</returns>
        private T BuildHotkeyTemplate<T>(IDictionary<string, string> values) where T : IHotkeyTemplate, new()
        {
            var hotkeyTemplate = new T();
            foreach (var option in hotkeyTemplate.HotkeyOptions)
            {   
                if (values.ContainsKey(option.KeyName))
                {
                    switch (option.InputType)
                    {
                        case InputOptionType.KeyboardKey:
                            option.Value =
                                new HotkeyTrigger((KeyboardKey) Enum.Parse(typeof(KeyboardKey), values[option.KeyName]));
                            break;
                        case InputOptionType.ControllerElement:
                        case InputOptionType.ControllerElementAxes:
                             option.Value =
                                new HotkeyTrigger((KeyboardKey)Enum.Parse(typeof(ControllerElement), values[option.KeyName]));
                            break;
                        default:
                            option.Value = new HotkeyTrigger();
                            break;
                    }
                }
            }
            return hotkeyTemplate;
        }

        private class HotkeyTemplateRecord
        {

            internal string TemplateFilename { get; set; }
            internal string SectionName { get; set; }
            internal string TemplateType { get; set; }
            internal string OptionKey { get; set; }
            internal string OptionValue { get; set; }
        }
    }
}
