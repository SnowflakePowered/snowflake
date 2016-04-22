#!/usr/bin/python3.4

import sys
import os
from decimal import Decimal
from collections import defaultdict

hotkey_template = '        [HotkeyOption("{0}", InputOptionType.{1}, DisplayName = "{2}")]'
input_template = '        [InputOption("{0}", InputOptionType.{1}, ControllerElement.{2})]'
prop_template =  '       public IMappedControllerElement {0} {{ get; set; }}'

def main(file):
    sections = defaultdict(list)
    lines = []
    with (open(file)) as f:
        content = f.readlines()
        for line in content:
            optionname, friendly, pascal, optionname, type, ishotkey = get_config_line(line)
            lines.append(hotkey_template.format(optionname, type, friendly) if ishotkey else input_template.format(optionname, type, get_ctrl_elem(optionname)))
            lines.append(prop_template.format(pascal))
    input = '\n'.join(lines)
    with open('retroarchinput.cs', 'w+') as f:
        print(input, file=f)
        
def get_config_line(line):
    optionname = line[:line.index(' =')]
    value = line[line.index('"') + 1:-2]
    friendly = make_friendly(optionname)
    pascal = make_pascal_case(optionname).replace("Player1", "Player")
    ishotkey = "player1" not in optionname
    optionname = optionname.replace("player1", "player{N}")
    type = "ControllerElement" if optionname.endswith('btn') else "ControllerElementAxes" if optionname.endswith('axis') else "KeyboardKey"
    return optionname, friendly, pascal, optionname, type, ishotkey

def get_ctrl_elem(optionname):
    optionname = optionname.replace("_btn", "")
    optionname = optionname.replace("_axis", "")
    if optionname[-1] in ['a', 'b', 'x', 'y', 'l', 'r']:
        return "Button"+optionname[-1].upper()
    if optionname.endswith("_up"):
        return "DirectionalN"
    if optionname.endswith("_down"):
        return "DirectionalS"
    if optionname.endswith("_left"):
        return "DirectionalW"
    if optionname.endswith("_right"):
        return "DirectionalE"
    if optionname.endswith("_l2"):
        return "TriggerLeft"
    if optionname.endswith("_r2"):
        return "TriggerRight"
    if optionname.endswith("_l3"):
        return "ButtonClickL"
    if optionname.endswith("_r3"):
        return "ButtonClickR"
    if optionname.endswith("_l_x_plus"):
        return "AxisLeftAnalogPositiveX"
    if optionname.endswith("_l_x_minus"):
        return "AxisLeftAnalogNegativeX"
    if optionname.endswith("_l_y_plus"):
        return "AxisLeftAnalogPositiveY"
    if optionname.endswith("_l_y_minus"):
        return "AxisLeftAnalogNegativeY"
    if optionname.endswith("_r_x_plus"):
        return "AxisRightAnalogPositiveX"
    if optionname.endswith("_r_x_minus"):
        return "AxisRightAnalogNegativeX"
    if optionname.endswith("_r_y_plus"):
        return "AxisRightAnalogPositiveY"
    if optionname.endswith("_r_y_minus"):
        return "AxisRightAnalogNegativeY"
    if optionname.endswith('start'):
        return "ButtonStart"
    if optionname.endswith('select'):
        return "ButtonSelect"
    if optionname.endswith("guide"):
        return "ButtonGuide"
    return "NoElement"
        
def make_friendly(optionname):
    return optionname.replace('_', ' ').title()

def make_pascal_case(optionname):
    return make_friendly(optionname).replace(' ', '')

if __name__ == "__main__":
    main('retroarch_input.cfg')
