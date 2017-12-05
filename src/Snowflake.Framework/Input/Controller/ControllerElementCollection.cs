using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;

namespace Snowflake.Input.Controller
{
    public class ControllerElementCollection : IControllerElementCollection
    {
        // standard + extended face buttons

        /// <inheritdoc/>
        public IControllerElementInfo ButtonA => this[ControllerElement.ButtonA];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonB => this[ControllerElement.ButtonB];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonC => this[ControllerElement.ButtonC];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonX => this[ControllerElement.ButtonX];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonY => this[ControllerElement.ButtonY];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonZ => this[ControllerElement.ButtonZ];

        // shoulders

        /// <inheritdoc/>
        public IControllerElementInfo ButtonL => this[ControllerElement.ButtonL];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonR => this[ControllerElement.ButtonR];

        // start/select/guide

        /// <inheritdoc/>
        public IControllerElementInfo ButtonStart => this[ControllerElement.ButtonStart];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonSelect => this[ControllerElement.ButtonSelect];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonGuide => this[ControllerElement.ButtonGuide];

        // used for mouse buttons and analog stick clicks

        /// <inheritdoc/>
        public IControllerElementInfo ButtonClickL => this[ControllerElement.ButtonClickL];

        /// <inheritdoc/>
        public IControllerElementInfo ButtonClickR => this[ControllerElement.ButtonClickR];

        // misc use

        /// <inheritdoc/>
        public IControllerElementInfo Button0 => this[ControllerElement.Button0];

        /// <inheritdoc/>
        public IControllerElementInfo Button1 => this[ControllerElement.Button1];

        /// <inheritdoc/>
        public IControllerElementInfo Button2 => this[ControllerElement.Button2];

        /// <inheritdoc/>
        public IControllerElementInfo Button3 => this[ControllerElement.Button3];

        /// <inheritdoc/>
        public IControllerElementInfo Button4 => this[ControllerElement.Button4];

        /// <inheritdoc/>
        public IControllerElementInfo Button5 => this[ControllerElement.Button5];

        /// <inheritdoc/>
        public IControllerElementInfo Button6 => this[ControllerElement.Button6];

        /// <inheritdoc/>
        public IControllerElementInfo Button7 => this[ControllerElement.Button7];

        /// <inheritdoc/>
        public IControllerElementInfo Button8 => this[ControllerElement.Button8];

        /// <inheritdoc/>
        public IControllerElementInfo Button9 => this[ControllerElement.Button9];

        /// <inheritdoc/>
        public IControllerElementInfo Button10 => this[ControllerElement.Button10];

        /// <inheritdoc/>
        public IControllerElementInfo Button11 => this[ControllerElement.Button11];

        /// <inheritdoc/>
        public IControllerElementInfo Button12 => this[ControllerElement.Button12];

        /// <inheritdoc/>
        public IControllerElementInfo Button13 => this[ControllerElement.Button13];

        /// <inheritdoc/>
        public IControllerElementInfo Button14 => this[ControllerElement.Button14];

        /// <inheritdoc/>
        public IControllerElementInfo Button15 => this[ControllerElement.Button15];

        /// <inheritdoc/>
        public IControllerElementInfo Button16 => this[ControllerElement.Button16];

        /// <inheritdoc/>
        public IControllerElementInfo Button17 => this[ControllerElement.Button17];

        /// <inheritdoc/>
        public IControllerElementInfo Button18 => this[ControllerElement.Button18];

        /// <inheritdoc/>
        public IControllerElementInfo Button19 => this[ControllerElement.Button19];

        /// <inheritdoc/>
        public IControllerElementInfo Button20 => this[ControllerElement.Button20];

        /// <inheritdoc/>
        public IControllerElementInfo Button21 => this[ControllerElement.Button21];

        /// <inheritdoc/>
        public IControllerElementInfo Button22 => this[ControllerElement.Button22];

        /// <inheritdoc/>
        public IControllerElementInfo Button23 => this[ControllerElement.Button23];

        /// <inheritdoc/>
        public IControllerElementInfo Button24 => this[ControllerElement.Button24];

        /// <inheritdoc/>
        public IControllerElementInfo Button25 => this[ControllerElement.Button25];

        /// <inheritdoc/>
        public IControllerElementInfo Button26 => this[ControllerElement.Button26];

        /// <inheritdoc/>
        public IControllerElementInfo Button27 => this[ControllerElement.Button27];

        /// <inheritdoc/>
        public IControllerElementInfo Button28 => this[ControllerElement.Button28];

        /// <inheritdoc/>
        public IControllerElementInfo Button29 => this[ControllerElement.Button29];

        /// <inheritdoc/>
        public IControllerElementInfo Button30 => this[ControllerElement.Button30];

        /// <inheritdoc/>
        public IControllerElementInfo Button31 => this[ControllerElement.Button31];

        // 8-way direciotnal

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalN => this[ControllerElement.DirectionalN];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalE => this[ControllerElement.DirectionalE];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalS => this[ControllerElement.DirectionalS];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalW => this[ControllerElement.DirectionalW];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalNE => this[ControllerElement.DirectionalNE];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalNW => this[ControllerElement.DirectionalNW];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalSE => this[ControllerElement.DirectionalSE];

        /// <inheritdoc/>
        public IControllerElementInfo DirectionalSW => this[ControllerElement.DirectionalSW];

        // two ananlog triggers

        /// <inheritdoc/>
        public IControllerElementInfo TriggerLeft => this[ControllerElement.TriggerLeft];

        /// <inheritdoc/>
        public IControllerElementInfo TriggerRight => this[ControllerElement.TriggerRight];

        // left analog directions

        /// <inheritdoc/>
        public IControllerElementInfo AxisLeftAnalogPositiveX => this[ControllerElement.AxisLeftAnalogPositiveX];

        /// <inheritdoc/>
        public IControllerElementInfo AxisLeftAnalogNegativeX => this[ControllerElement.AxisLeftAnalogNegativeX];

        /// <inheritdoc/>
        public IControllerElementInfo AxisLeftAnalogPositiveY => this[ControllerElement.AxisLeftAnalogPositiveY];

        /// <inheritdoc/>
        public IControllerElementInfo AxisLeftAnalogNegativeY => this[ControllerElement.AxisLeftAnalogNegativeY];

        // right analog directions

        /// <inheritdoc/>
        public IControllerElementInfo AxisRightAnalogPositiveX => this[ControllerElement.AxisRightAnalogPositiveX];

        /// <inheritdoc/>
        public IControllerElementInfo AxisRightAnalogNegativeX => this[ControllerElement.AxisRightAnalogNegativeX];

        /// <inheritdoc/>
        public IControllerElementInfo AxisRightAnalogPositiveY => this[ControllerElement.AxisRightAnalogPositiveY];

        /// <inheritdoc/>
        public IControllerElementInfo AxisRightAnalogNegativeY => this[ControllerElement.AxisRightAnalogNegativeY];

        // rumble

        /// <inheritdoc/>
        public IControllerElementInfo RumbleBig => this[ControllerElement.RumbleBig];

        /// <inheritdoc/>
        public IControllerElementInfo RumbleSmall => this[ControllerElement.RumbleSmall];

        // mouse pointer support

        /// <inheritdoc/>
        public IControllerElementInfo Pointer2D => this[ControllerElement.Pointer2D];

        /// <inheritdoc/>
        public IControllerElementInfo Pointer3D => this[ControllerElement.Pointer3D];

        // pointer axes (wii remote and mouse)

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisPositiveX => this[ControllerElement.PointerAxisPositiveX];

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisNegativeX => this[ControllerElement.PointerAxisNegativeX];

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisPositiveY => this[ControllerElement.PointerAxisPositiveY];

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisNegativeY => this[ControllerElement.PointerAxisNegativeY];

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisPositiveZ => this[ControllerElement.PointerAxisPositiveZ];

        /// <inheritdoc/>
        public IControllerElementInfo PointerAxisNegativeZ => this[ControllerElement.PointerAxisNegativeZ];

        // keyboard layout

        /// <inheritdoc/>
        public IControllerElementInfo Keyboard => this[ControllerElement.Keyboard];

        /// <inheritdoc/>
        public IControllerElementInfo Touchscreen => this[ControllerElement.Touchscreen];

        private readonly IDictionary<ControllerElement, IControllerElementInfo> controllerElements;

        /// <inheritdoc/>
        public IControllerElementInfo this[ControllerElement element]
        {
            get
            {
                if (element.IsKeyboardKey())
                {
                    return this.Keyboard;
                }

                if (this.controllerElements.ContainsKey(element) || element == ControllerElement.NoElement)
                {
                    return this.controllerElements[element];
                }

                return null;
            }
        }

        public ControllerElementCollection()
        {
            this.controllerElements = new Dictionary<ControllerElement, IControllerElementInfo>();
        }

        internal void Add(ControllerElement elementKey, IControllerElementInfo info)
        {
            this.controllerElements[elementKey] = info;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<ControllerElement, IControllerElementInfo>> GetEnumerator()
        {
            return this.controllerElements.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
