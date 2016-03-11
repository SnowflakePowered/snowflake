namespace Snowflake.Controller
{
    /// <summary>
    /// Represents a standard gamepad
    /// 2 shoulder buttons
    /// 2 shoulder triggers
    /// 1 4-cardinal direction d-pad
    /// 2 clickable analog sticks 
    /// Start Button
    /// Select Button
    /// </summary>
    public interface IGamepadAbstraction
    {
        /// <summary>
        /// The name of the device that maps to this abstraction
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// The type of profile of the controller
        /// </summary>
        ControllerProfileType ProfileType { get; }

        /// <summary>
        /// Left Shoulder Button
        /// </summary>
        string L1 { get; set; }

        /// <summary>
        /// Left Trigger
        /// </summary>
        string L2 { get; set; }

        /// <summary>
        /// Left Stick Depress
        /// </summary>
        string L3 { get; set; }

        /// <summary>
        /// Right Shoulder Button
        /// </summary>
        string R1 { get; set; }
        
        /// <summary>
        /// Right Trigger
        /// </summary>
        string R2 { get; set; }

        /// <summary>
        /// Right Stick Depress
        /// </summary>
        string R3 { get; set; }

        /// <summary>
        /// Dpad Up
        /// </summary>
        string DpadUp { get; set;}

        /// <summary>
        /// Dpad Down
        /// </summary>
        string DpadDown { get; set; }

        /// <summary>
        /// DpadLeft
        /// </summary>
        string DpadLeft { get; set; }

        /// <summary>
        /// DpadRight
        /// </summary>
        string DpadRight { get; set; }

        /// <summary>
        /// Right stick horizontal movement left
        /// </summary>
        string RightAnalogXLeft { get; set; }

        /// <summary>
        /// RIght stick horizontal movement right
        /// </summary>
        string RightAnalogXRight { get; set; }

        /// <summary>
        /// Right stick vertical movement up
        /// </summary>
        string RightAnalogYUp { get; set; }

        /// <summary>
        /// Right stick vertical movement down
        /// </summary>
        string RightAnalogYDown { get; set; }

        /// <summary>
        /// Left stick horizontal movement left
        /// </summary>
        string LeftAnalogXLeft { get; set; }

        /// <summary>
        /// Left stick horizontal movement right
        /// </summary>
        string LeftAnalogXRight { get; set; }

        /// <summary>
        /// Left stick vertical movement up
        /// </summary>
        string LeftAnalogYUp { get; set; }
        
        /// <summary>
        /// Left stick vertical movement down
        /// </summary>
        string LeftAnalogYDown { get; set; }

        /// <summary>
        /// Select button
        /// </summary>
        string Select { get; set; }

        /// <summary>
        /// Start button
        /// </summary>
        string Start { get; set; }

        /// <summary>
        /// A Button
        /// </summary>
        string A { get; set; }

        /// <summary>
        /// B Button
        /// </summary>
        string B { get; set; }

        /// <summary>
        /// X Button
        /// </summary>
        string X { get; set; }

        /// <summary>
        /// Y button
        /// </summary>
        string Y { get; set; }

        /// <summary>
        /// Indexer accessor to get a certain key
        /// </summary>
        /// <param name="keyName">The name of the key</param>
        /// <returns>The value of the key</returns>
        string this[string keyName] { get; set; }
    }
}
