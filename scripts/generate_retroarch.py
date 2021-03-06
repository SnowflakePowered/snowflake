#!/usr/bin/python3.4

import sys
import os
from decimal import Decimal
from collections import defaultdict
template = u'''       [ConfigurationOption("{0}", DisplayName = "{1}"{5})]
       public {2} {3} {{ get; set; }} = {4};'''

classtemplate = u'''using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

//autogenerated using generate_retroarch.py
namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Configuration
{{
    public class {0}Configuration : ConfigurationSection
    {{
{1}

       public {0}Configuration() : base ("{2}", "{0} Options", "retroarch.cfg")
       {{

       }}
       
     }}
}}'''

addtemplate = '            config.Add<{0}, RetroArchConfigSerializer>("{1}", new {0}());'
gettemplate = '        public {0} {0} => this.Get<{0}>("{1}");'
collectionmaketemplate = '''using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{{
    //autogenerated using generate_retroarch.py
    public partial class RetroArchConfiguration
    {{
        public static RetroArchConfiguration MakeDefault()
        {{
            var config = new RetroArchConfiguration();
{0}
            return config;
        }}
    }}
}}
'''

collectionaccesstemplate = '''using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{{
    //autogenerated using generate_retroarch.py
    public partial class RetroArchConfiguration : ConfigurationCollection
    {{
{0}
    }}
}}
'''
strtodo = '       //todo check if enum value'
inttodo = '       //todo check max'


def main(file):
    os.makedirs(os.path.dirname('./retroarch/'), exist_ok=True)
    sections = defaultdict(list)
    with (open(file)) as f:
        content = f.readlines()
        for line in content:
            sectionname, optionname, value, friendly, pascal, type = get_config_line(
                line)
            if sectionname in ["audio", "video"]:
                continue

            if type == 'string':
                value = '"{0}"'.format(value)
            option = template.format(optionname, friendly, type, pascal, value,
                                     ", FilePath = true, Private = true" if sectionname == "directory" else "")
            if type == 'string':
                option = '\n'.join([strtodo, option])
            if type == 'int':
                option = '\n'.join([inttodo, option])
            sections[sectionname].append(option)
    for section, options in sections.items():
        optionsclass = classtemplate.format(
            section.title(), '\n'.join(options), section)
        with open(''.join(['./retroarch/', section.title(), 'Configuration.cs']), 'w+') as f:
            print(optionsclass, file=f)
    collectionmake = collectionmaketemplate.format('\n'.join(
        addtemplate.format(''.join([t.title(), 'Configuration']), t) for t in sections))
    collectionaccess = collectionaccesstemplate.format('\n'.join(
        gettemplate.format(''.join([t.title(), 'Configuration']), t) for t in sections))
    with open(''.join(['./retroarch/', 'RetroArchConfigurationCollection.cs']), 'w+') as f:
        print(collectionaccess, file=f)
    with open(''.join(['./retroarch/', 'RetroArchConfigurationCollection.Make.cs']), 'w+') as f:
        print(collectionmake, file=f)


def get_config_line(line):
    sectionname = line[:line.index('_')]
    optionname = line[:line.index(' =')]
    value = line[line.index('"') + 1:-2]
    friendly = make_friendly(optionname)
    pascal = make_pascal_case(optionname)
    type = get_type(value)
    if True in [True for match in ["directory", "dir", "path"] if match in optionname]:
        sectionname = "directory"
    if sectionname == "aspect":
        sectionname = "video"
    if True in [True for match in ["save", "sram"] if match in optionname]:
        sectionname = "save"
    if "log" in sectionname:
        sectionname = "log"
    return sectionname, optionname, value, friendly, pascal, type


def make_friendly(optionname):
    return optionname.replace('_', ' ').title()


def make_pascal_case(optionname):
    return make_friendly(optionname).replace(' ', '')


def get_type(value):
    try:
        int(value)
        return 'int'
    except ValueError:
        pass
    try:
        Decimal(value)
        return 'double'
    except:
        pass
    if value in ['true', 'false']:
        return 'bool'
    return 'string'


if __name__ == "__main__":
    main('retroarch_clean.cfg')
