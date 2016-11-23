﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    public class ControllerElementInfo : IControllerElementInfo
    {
        public string Label { get; }
        public ControllerElementType Type { get; }

        public ControllerElementInfo(string label, ControllerElementType type)
        {
            this.Label = label;
            this.Type = type;
        }
    }
}
