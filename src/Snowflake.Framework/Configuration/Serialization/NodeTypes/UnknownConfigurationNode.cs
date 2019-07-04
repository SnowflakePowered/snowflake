﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
   public sealed class UnknownConfigurationNode
        : AbstractConfigurationNode<object>
    {
        internal UnknownConfigurationNode(string key, object value) : base(key, value)
        {
        }
    }
}
