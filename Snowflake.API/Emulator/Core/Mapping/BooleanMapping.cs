using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Core.Mapping
{
    public class BooleanMapping
    {
        public readonly EmulatorBoolean TRUE;
        public readonly EmulatorBoolean FALSE;

        public BooleanMapping(string trueValue, string falseValue)
        {
            this.TRUE = new EmulatorBoolean(trueValue, true);
            this.FALSE = new EmulatorBoolean(falseValue, false);
        }

        public EmulatorBoolean FromBool(bool value)
        {
            if (value) 
                return this.TRUE;
            else 
                return this.FALSE;
        }
    }
    /// <summary>
    /// Represents a boolean in an emulator configuration file
    /// </summary>
    public class EmulatorBoolean
    {
        private string representation;
        private bool value;

        protected internal EmulatorBoolean(string representation, bool value)
        {
            this.representation = representation;
            this.value = value;
        }
        protected internal EmulatorBoolean()
        {
            throw new InvalidOperationException("You must define a value and represenation");
        }
        public override string ToString()
        {
            return this.representation;
        }


        //Override operators to allow in place of bool
        public static implicit operator bool(EmulatorBoolean emulatorBoolean)
        {
            return emulatorBoolean.value;
        }
    }
}
