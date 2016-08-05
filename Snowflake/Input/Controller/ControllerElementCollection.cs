using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    public class ControllerElementCollection : IControllerElementCollection
    {
        //standard + extended face buttons
        public IControllerElementInfo ButtonA => this[ControllerElement.ButtonA];
        public IControllerElementInfo ButtonB => this[ControllerElement.ButtonB];
        public IControllerElementInfo ButtonC => this[ControllerElement.ButtonC];

        public IControllerElementInfo ButtonX => this[ControllerElement.ButtonX];
        public IControllerElementInfo ButtonY => this[ControllerElement.ButtonY];
        public IControllerElementInfo ButtonZ => this[ControllerElement.ButtonZ];

        //shoulders
        public IControllerElementInfo ButtonL => this[ControllerElement.ButtonL];
        public IControllerElementInfo ButtonR => this[ControllerElement.ButtonR];

        //start/select/guide
        public IControllerElementInfo ButtonStart => this[ControllerElement.ButtonStart];
        public IControllerElementInfo ButtonSelect => this[ControllerElement.ButtonSelect];
        public IControllerElementInfo ButtonGuide => this[ControllerElement.ButtonGuide];

        //used for mouse buttons and analog stick clicks
        public IControllerElementInfo ButtonClickL => this[ControllerElement.ButtonClickL];
        public IControllerElementInfo ButtonClickR => this[ControllerElement.ButtonClickR];

        //misc use
        public IControllerElementInfo Button0 => this[ControllerElement.Button0];
        public IControllerElementInfo Button1 => this[ControllerElement.Button1];
        public IControllerElementInfo Button2 => this[ControllerElement.Button2];
        public IControllerElementInfo Button3 => this[ControllerElement.Button3];
        public IControllerElementInfo Button4 => this[ControllerElement.Button4];
        public IControllerElementInfo Button5 => this[ControllerElement.Button5];
        public IControllerElementInfo Button6 => this[ControllerElement.Button6];
        public IControllerElementInfo Button7 => this[ControllerElement.Button7];
        public IControllerElementInfo Button8 => this[ControllerElement.Button8];
        public IControllerElementInfo Button9 => this[ControllerElement.Button9];
        public IControllerElementInfo Button10 => this[ControllerElement.Button10];
        public IControllerElementInfo Button11 => this[ControllerElement.Button11];
        public IControllerElementInfo Button12 => this[ControllerElement.Button12];
        public IControllerElementInfo Button13 => this[ControllerElement.Button13];
        public IControllerElementInfo Button14 => this[ControllerElement.Button14];
        public IControllerElementInfo Button15 => this[ControllerElement.Button15];
        public IControllerElementInfo Button16 => this[ControllerElement.Button16];
        public IControllerElementInfo Button17 => this[ControllerElement.Button17];
        public IControllerElementInfo Button18 => this[ControllerElement.Button18];
        public IControllerElementInfo Button19 => this[ControllerElement.Button19];
        public IControllerElementInfo Button20 => this[ControllerElement.Button20];
        public IControllerElementInfo Button21 => this[ControllerElement.Button21];
        public IControllerElementInfo Button22 => this[ControllerElement.Button22];
        public IControllerElementInfo Button23 => this[ControllerElement.Button23];
        public IControllerElementInfo Button24 => this[ControllerElement.Button24];
        public IControllerElementInfo Button25 => this[ControllerElement.Button25];
        public IControllerElementInfo Button26 => this[ControllerElement.Button26];
        public IControllerElementInfo Button27 => this[ControllerElement.Button27];
        public IControllerElementInfo Button28 => this[ControllerElement.Button28];
        public IControllerElementInfo Button29 => this[ControllerElement.Button29];
        public IControllerElementInfo Button30 => this[ControllerElement.Button30];
        public IControllerElementInfo Button31 => this[ControllerElement.Button31];

        //8-way direciotnal 
        public IControllerElementInfo DirectionalN => this[ControllerElement.DirectionalN];
        public IControllerElementInfo DirectionalE => this[ControllerElement.DirectionalE];
        public IControllerElementInfo DirectionalS => this[ControllerElement.DirectionalS];
        public IControllerElementInfo DirectionalW => this[ControllerElement.DirectionalW];
        public IControllerElementInfo DirectionalNE => this[ControllerElement.DirectionalNE];
        public IControllerElementInfo DirectionalNW => this[ControllerElement.DirectionalNW];
        public IControllerElementInfo DirectionalSE => this[ControllerElement.DirectionalSE];
        public IControllerElementInfo DirectionalSW => this[ControllerElement.DirectionalSW];

        //two ananlog triggers
        public IControllerElementInfo TriggerLeft => this[ControllerElement.TriggerLeft];
        public IControllerElementInfo TriggerRight => this[ControllerElement.TriggerRight];

        //left analog directions
        public IControllerElementInfo AxisLeftAnalogPositiveX => this[ControllerElement.AxisLeftAnalogPositiveX];
        public IControllerElementInfo AxisLeftAnalogNegativeX => this[ControllerElement.AxisLeftAnalogNegativeX];
        public IControllerElementInfo AxisLeftAnalogPositiveY => this[ControllerElement.AxisLeftAnalogPositiveY];
        public IControllerElementInfo AxisLeftAnalogNegativeY => this[ControllerElement.AxisLeftAnalogNegativeY];

        //right analog directions
        public IControllerElementInfo AxisRightAnalogPositiveX => this[ControllerElement.AxisRightAnalogPositiveX];
        public IControllerElementInfo AxisRightAnalogNegativeX => this[ControllerElement.AxisRightAnalogNegativeX];
        public IControllerElementInfo AxisRightAnalogPositiveY => this[ControllerElement.AxisRightAnalogPositiveY];
        public IControllerElementInfo AxisRightAnalogNegativeY => this[ControllerElement.AxisRightAnalogNegativeY];

        //rumble
        public IControllerElementInfo RumbleBig => this[ControllerElement.RumbleBig];
        public IControllerElementInfo RumbleSmall => this[ControllerElement.RumbleSmall];

        //mouse pointer support
        public IControllerElementInfo Pointer2D => this[ControllerElement.Pointer2D];
        public IControllerElementInfo Pointer3D => this[ControllerElement.Pointer3D];

        //pointer axes (wii remote and mouse)
        public IControllerElementInfo PointerAxisPositiveX => this[ControllerElement.PointerAxisPositiveX];
        public IControllerElementInfo PointerAxisNegativeX => this[ControllerElement.PointerAxisNegativeX];
        public IControllerElementInfo PointerAxisPositiveY => this[ControllerElement.PointerAxisPositiveY];
        public IControllerElementInfo PointerAxisNegativeY => this[ControllerElement.PointerAxisNegativeY];
        public IControllerElementInfo PointerAxisPositiveZ => this[ControllerElement.PointerAxisPositiveZ];
        public IControllerElementInfo PointerAxisNegativeZ => this[ControllerElement.PointerAxisNegativeZ];

        //keyboard layout
        public IControllerElementInfo Keyboard => this[ControllerElement.Keyboard];
        public IControllerElementInfo Touchscreen => this[ControllerElement.Touchscreen];

        private readonly IDictionary<ControllerElement, IControllerElementInfo> controllerElements;

        public IControllerElementInfo this[ControllerElement element] => this.controllerElements.ContainsKey(element) ? this.controllerElements[element] : null;

        public ControllerElementCollection()
        {
            this.controllerElements = new Dictionary<ControllerElement, IControllerElementInfo>();
        }

        internal void Add(ControllerElement elementKey, IControllerElementInfo info)
        {
            this.controllerElements[elementKey] = info;
        }

        public IEnumerator<KeyValuePair<ControllerElement, IControllerElementInfo>> GetEnumerator()
        {
            return this.controllerElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
