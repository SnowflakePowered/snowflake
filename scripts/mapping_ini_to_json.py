#!/bin/python3

import configparser
import json

config = configparser.ConfigParser()
config.optionxform = str 

with open('retroarch-mapping.ini', 'r') as f:
    config.read_file(f)
    print(json.dumps(config._sections, indent=4, separators=(',', ': ')))