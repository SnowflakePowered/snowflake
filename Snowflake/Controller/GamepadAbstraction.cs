namespace Snowflake.Controller
{
    public class GamepadAbstraction : IGamepadAbstraction
    {
        public string DeviceName { get; }
        public ControllerProfileType ProfileType { get; }
        public GamepadAbstraction(string deviceName, ControllerProfileType profileType)
        {
            this.DeviceName = deviceName;
            this.ProfileType = profileType;
        }
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }
        public string R3 { get; set; }

        public string DpadUp { get; set; }
        public string DpadDown { get; set; }
        public string DpadLeft { get; set; }
        public string DpadRight { get; set; }

        public string RightAnalogXLeft { get; set; }
        public string RightAnalogXRight { get; set; }
        public string RightAnalogYUp { get; set; }
        public string RightAnalogYDown { get; set; }

        public string LeftAnalogXLeft { get; set; }
        public string LeftAnalogXRight { get; set; }
        public string LeftAnalogYUp { get; set; }
        public string LeftAnalogYDown { get; set; }

        public string Select { get; set; }
        public string Start { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string X { get; set; }
        public string Y { get; set; }

        public string this[string keyName]
        {
            get
            {
                string _keyName = keyName.ToLowerInvariant(); //prevent case issues
                switch (_keyName)
                {
                    case "l1":
                        return this.L1;
                    case "l2":
                        return this.L2;
                    case "l3":
                        return this.L3;
                    case "r1":
                        return this.R1;
                    case "r2":
                        return this.R2;
                    case "r3":
                        return this.R3;
                    case "dpadup":
                        return this.DpadUp;
                    case "dpaddown":
                        return this.DpadDown;
                    case "dpadleft":
                        return this.DpadLeft;
                    case "dpadright":
                        return this.DpadRight;
                    case "rightanalogxleft":
                        return this.RightAnalogXLeft;
                    case "rightanalogxright":
                        return this.RightAnalogXRight;
                    case "rightanalogyup":
                        return this.RightAnalogYUp;
                    case "rightanalogydown":
                        return this.RightAnalogYDown;
                    case "leftanalogxleft":
                        return this.LeftAnalogXLeft;
                    case "leftanalogxright":
                        return this.LeftAnalogXRight;
                    case "leftanalogyup":
                        return this.LeftAnalogYUp;
                    case "leftanalogydown":
                        return this.LeftAnalogYDown;
                    case "a":
                        return this.A;
                    case "b":
                        return this.B;
                    case "x":
                        return this.X;
                    case "y":
                        return this.Y;
                    case "start":
                        return this.Start;
                    case "select":
                        return this.Select;
                    default:
                        return string.Empty;
                }
            }
            set
            {
                string _keyName = keyName.ToLowerInvariant(); //prevent case issues
                switch (_keyName)
                {
                    case "l1":
                        this.L1 = value; break;
                    case "l2":
                        this.L2 = value; break;
                    case "l3":
                        this.L3 = value; break;
                    case "r1":
                        this.R1 = value; break;
                    case "r2":
                        this.R2 = value; break;
                    case "r3":
                        this.R3 = value; break;
                    case "dpadup":
                        this.DpadUp = value; break;
                    case "dpaddown":
                        this.DpadDown = value; break;
                    case "dpadleft":
                        this.DpadLeft = value; break;
                    case "dpadright":
                        this.DpadRight = value; break;
                    case "rightanalogxleft":
                        this.RightAnalogXLeft = value; break;
                    case "rightanalogxright":
                        this.RightAnalogXRight = value; break;
                    case "rightanalogyup":
                        this.RightAnalogYUp = value; break;
                    case "rightanalogydown":
                        this.RightAnalogYDown = value; break;
                    case "leftanalogxleft":
                        this.LeftAnalogXLeft = value; break;
                    case "leftanalogxright":
                        this.LeftAnalogXRight = value; break;
                    case "leftanalogyup":
                        this.LeftAnalogYUp = value; break;
                    case "leftanalogydown":
                        this.LeftAnalogYDown = value; break;
                    case "a":
                        this.A = value; break;
                    case "b":
                        this.B = value; break;
                    case "x":
                        this.X = value; break;
                    case "y":
                        this.Y = value; break;
                    case "start":
                        this.Start = value; break;
                    case "select":
                        this.Select = value; break;
                    default:
                        break;

                }
            }
        }
    }
}
