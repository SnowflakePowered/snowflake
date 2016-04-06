using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    public interface IControllerElementCollection : IEnumerable<KeyValuePair<ControllerElement, IControllerElementInfo>>
    {
        IControllerElementInfo ButtonA { get; }
        IControllerElementInfo ButtonB { get; }
        IControllerElementInfo ButtonC { get; }
        IControllerElementInfo ButtonX { get; }
        IControllerElementInfo ButtonY { get; }
        IControllerElementInfo ButtonZ { get; }
        IControllerElementInfo ButtonL { get; }
        IControllerElementInfo ButtonR { get; }
        IControllerElementInfo ButtonStart { get; }
        IControllerElementInfo ButtonSelect { get; }
        IControllerElementInfo ButtonGuide { get; }
        IControllerElementInfo ButtonClickL { get; }
        IControllerElementInfo ButtonClickR { get; }
        IControllerElementInfo Button0 { get; }
        IControllerElementInfo Button1 { get; }
        IControllerElementInfo Button2 { get; }
        IControllerElementInfo Button3 { get; }
        IControllerElementInfo Button4 { get; }
        IControllerElementInfo Button5 { get; }
        IControllerElementInfo Button6 { get; }
        IControllerElementInfo Button7 { get; }
        IControllerElementInfo Button8 { get; }
        IControllerElementInfo Button9 { get; }
        IControllerElementInfo Button10 { get; }
        IControllerElementInfo Button11 { get; }
        IControllerElementInfo Button12 { get; }
        IControllerElementInfo Button13 { get; }
        IControllerElementInfo Button14 { get; }
        IControllerElementInfo Button15 { get; }
        IControllerElementInfo Button16 { get; }
        IControllerElementInfo Button17 { get; }
        IControllerElementInfo Button18 { get; }
        IControllerElementInfo Button19 { get; }
        IControllerElementInfo Button20 { get; }
        IControllerElementInfo Button21 { get; }
        IControllerElementInfo Button22 { get; }
        IControllerElementInfo Button23 { get; }
        IControllerElementInfo Button24 { get; }
        IControllerElementInfo Button25 { get; }
        IControllerElementInfo Button26 { get; }
        IControllerElementInfo Button27 { get; }
        IControllerElementInfo Button28 { get; }
        IControllerElementInfo Button29 { get; }
        IControllerElementInfo Button30 { get; }
        IControllerElementInfo Button31 { get; }
        IControllerElementInfo DirectionalN { get; }
        IControllerElementInfo DirectionalE { get; }
        IControllerElementInfo DirectionalS { get; }
        IControllerElementInfo DirectionalW { get; }
        IControllerElementInfo DirectionalNE { get; }
        IControllerElementInfo DirectionalNW { get; }
        IControllerElementInfo DirectionalSE { get; }
        IControllerElementInfo DirectionalSW { get; }
        IControllerElementInfo TriggerLeft { get; }
        IControllerElementInfo TriggerRight { get; }
        IControllerElementInfo AxisLeftAnalogPositiveX { get; }
        IControllerElementInfo AxisLeftAnalogNegativeX { get; }
        IControllerElementInfo AxisLeftAnalogPositiveY { get; }
        IControllerElementInfo AxisLeftAnalogNegativeY { get; }
        IControllerElementInfo AxisRightAnalogPositiveX { get; }
        IControllerElementInfo AxisRightAnalogNegativeX { get; }
        IControllerElementInfo AxisRightAnalogPositiveY { get; }
        IControllerElementInfo AxisRightAnalogNegativeY { get; }
        IControllerElementInfo RumbleBig { get; }
        IControllerElementInfo RumbleSmall { get; }
        IControllerElementInfo PointerMouse { get; }
        IControllerElementInfo PointerAxisPositiveX { get; }
        IControllerElementInfo PointerAxisNegativeX { get; }
        IControllerElementInfo PointerAxisPositiveY { get; }
        IControllerElementInfo PointerAxisNegativeY { get; }
        IControllerElementInfo Keyboard { get; }
        IControllerElementInfo this[ControllerElement controllerElement] { get; }
    }
}
