using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                               "TemplateType TEXT",
                               "SectionName TEXT",
                               "OptionKey TEXT",
                               "KeyboardTrigger TEXT",
                               "ControllerTrigger TEXT",
                               "PRIMARY KEY (TemplateType, SectionName, OptionKey)");
        }

        public T GetTemplate<T>() where T : IHotkeyTemplate, new()
        {
            var values = this.GetValues<T>();
            return values.Any() ? this.BuildHotkeyTemplate<T>(values) : new T();
        }

        public void SetTemplate(IHotkeyTemplate template)
        {
            string templateType = template.GetType().Name;
            var values = from option in template.HotkeyOptions
                         select new
                         {
                             TemplateType = templateType,
                             SectionName = template.SectionName,
                             OptionKey = option.KeyName,
                             KeyboardTrigger = option.Value.KeyboardTrigger.ToString(),
                             ControllerTrigger = option.Value.ControllerTrigger.ToString()
                         };
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(
                    @"INSERT OR REPLACE INTO hotkeys (TemplateType, SectionName, OptionKey, KeyboardTrigger, ControllerTrigger) VALUES 
                    (@TemplateType, @SectionName, @OptionKey, @KeyboardTrigger, @ControllerTrigger)", values);
            });
        }


        private IDictionary<string, HotkeyTrigger> GetValues<T>() where T : IHotkeyTemplate, new()
        {
            return this.backingDatabase.Query<IDictionary<string, HotkeyTrigger>>(dbConnection =>
            {
                var retValues = new Dictionary<string, HotkeyTrigger>();
                var values = dbConnection.Query<HotkeyTemplateRecord>
                ("SELECT * FROM hotkeys WHERE TemplateType = @TemplateType",
                    new { TemplateType = typeof(T).Name });
                foreach (var options in values)
                {
                    retValues[options.OptionKey] = new HotkeyTrigger((KeyboardKey)Enum.Parse(typeof(KeyboardKey), options.KeyboardTrigger), 
                        (ControllerElement)Enum.Parse(typeof(ControllerElement), options.ControllerTrigger));
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
        private T BuildHotkeyTemplate<T>(IDictionary<string, HotkeyTrigger> values) where T : IHotkeyTemplate, new()
        {
            var hotkeyTemplate = new T();
            foreach (var option in hotkeyTemplate.HotkeyOptions)
            {
                if (values.ContainsKey(option.KeyName))
                    option.Value = values[option.KeyName];
            }
            return hotkeyTemplate;
        }

        private class HotkeyTemplateRecord
        {
            internal string TemplateType { get; set; }
            internal string SectionName { get; set; }
            internal string KeyboardTrigger { get; set; }
            internal string ControllerTrigger { get; set; }
            internal string OptionKey { get; set; }
        }
    }
}
