using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Input
{
    public class KeyboardMapping : IKeyboardMapping
    {
        public string KEY_A { get; private set; }
        public string KEY_B { get; private set; }
        public string KEY_C { get; private set; }
        public string KEY_D { get; private set; }
        public string KEY_E { get; private set; }
        public string KEY_F { get; private set; }
        public string KEY_G { get; private set; }
        public string KEY_H { get; private set; }
        public string KEY_I { get; private set; }
        public string KEY_J { get; private set; }
        public string KEY_K { get; private set; }
        public string KEY_L { get; private set; }
        public string KEY_M { get; private set; }
        public string KEY_N { get; private set; }
        public string KEY_O { get; private set; }
        public string KEY_P { get; private set; }
        public string KEY_Q { get; private set; }
        public string KEY_R { get; private set; }
        public string KEY_S { get; private set; }
        public string KEY_T { get; private set; }
        public string KEY_U { get; private set; }
        public string KEY_V { get; private set; }
        public string KEY_W { get; private set; }
        public string KEY_X { get; private set; }
        public string KEY_Y { get; private set; }
        public string KEY_Z { get; private set; }
        public string KEY_0 { get; private set; }
        public string KEY_1 { get; private set; }
        public string KEY_2 { get; private set; }
        public string KEY_3 { get; private set; }
        public string KEY_4 { get; private set; }
        public string KEY_5 { get; private set; }
        public string KEY_6 { get; private set; }
        public string KEY_7 { get; private set; }
        public string KEY_8 { get; private set; }
        public string KEY_9 { get; private set; }
        public string KEY_EQUALS { get; private set; }
        public string KEY_MINUS { get; private set; }
        public string KEY_BACKSPACE { get; private set; }
        public string KEY_SPACEBAR { get; private set; }
        public string KEY_ENTER { get; private set; }
        public string KEY_UP { get; private set; }
        public string KEY_DOWN { get; private set; }
        public string KEY_LEFT { get; private set; }
        public string KEY_RIGHT { get; private set; }
        public string KEY_TAB { get; private set; }
        public string KEY_INSERT { get; private set; }
        public string KEY_DELETE { get; private set; }
        public string KEY_HOME { get; private set; }
        public string KEY_END { get; private set; }
        public string KEY_PAGE_UP { get; private set; }
        public string KEY_PAGE_DOWN { get; private set; }
        public string KEY_SHIFT { get; private set; }
        public string KEY_CTRL { get; private set; }
        public string KEY_ALT { get; private set; }
        public string KEY_ESCAPE { get; private set; }
        public string KEY_TILDE { get; private set; }
        public string KEY_QUOTE { get; private set; }
        public string KEY_SEMICOLON { get; private set; }
        public string KEY_COMMA { get; private set; }
        public string KEY_PERIOD { get; private set; }
        public string KEY_SLASH { get; private set; }
        public string KEY_BRACKET_LEFT { get; private set; }
        public string KEY_BRACKET_RIGHT { get; private set; }
        public string KEY_BACKSLASH { get; private set; }
        public string KEY_RIGHT_ALT { get; private set; }
        public string KEY_RIGHT_CTRL { get; private set; }
        public string KEY_RIGHT_SHIFT { get; private set; }
        public string KEY_NUMPAD_0 { get; private set; }
        public string KEY_NUMPAD_1 { get; private set; }
        public string KEY_NUMPAD_2 { get; private set; }
        public string KEY_NUMPAD_3 { get; private set; }
        public string KEY_NUMPAD_4 { get; private set; }
        public string KEY_NUMPAD_5 { get; private set; }
        public string KEY_NUMPAD_6 { get; private set; }
        public string KEY_NUMPAD_7 { get; private set; }
        public string KEY_NUMPAD_8 { get; private set; }
        public string KEY_NUMPAD_9 { get; private set; }
        public string KEY_NUMPAD_PERIOD { get; private set; }
        public string KEY_NUMPAD_PLUS { get; private set; }
        public string KEY_NUMPAD_MINUS { get; private set; }
        public string KEY_NUMPAD_ENTER { get; private set; }
        public string KEY_F_1 { get; private set; }
        public string KEY_F_2 { get; private set; }
        public string KEY_F_3 { get; private set; }
        public string KEY_F_4 { get; private set; }
        public string KEY_F_5 { get; private set; }
        public string KEY_F_6 { get; private set; }
        public string KEY_F_7 { get; private set; }
        public string KEY_F_8 { get; private set; }
        public string KEY_F_9 { get; private set; }
        public string KEY_F_10 { get; private set; }
        public string KEY_F_11 { get; private set; }
        public string KEY_F_12 { get; private set; }
        public string MOUSE_Y_DOWN { get; private set; }
        public string MOUSE_Y_UP { get; private set; }
        public string MOUSE_X_LEFT { get; private set; }
        public string MOUSE_X_RIGHT { get; private set; }
        public string MOUSE_LCLICK { get; private set; }
        public string MOUSE_RCLICK { get; private set; }
        public string MOUSE_MCLICK { get; private set; }
        public string MOUSE_WHEELUP { get; private set; }
        public string MOUSE_WHEELDOWN { get; private set; }

        private IDictionary<string, string> mappingData;

        public KeyboardMapping(IDictionary<string, string> mappingData)
        {
            this.mappingData = mappingData;
            this.KEY_A = mappingData["KEY_A"];
            this.KEY_B = mappingData["KEY_B"];
            this.KEY_C = mappingData["KEY_C"];
            this.KEY_D = mappingData["KEY_D"];
            this.KEY_E = mappingData["KEY_E"];
            this.KEY_F = mappingData["KEY_F"];
            this.KEY_G = mappingData["KEY_G"];
            this.KEY_H = mappingData["KEY_H"];
            this.KEY_I = mappingData["KEY_I"];
            this.KEY_J = mappingData["KEY_J"];
            this.KEY_K = mappingData["KEY_K"];
            this.KEY_L = mappingData["KEY_L"];
            this.KEY_M = mappingData["KEY_M"];
            this.KEY_N = mappingData["KEY_N"];
            this.KEY_O = mappingData["KEY_O"];
            this.KEY_P = mappingData["KEY_P"];
            this.KEY_Q = mappingData["KEY_Q"];
            this.KEY_R = mappingData["KEY_R"];
            this.KEY_S = mappingData["KEY_S"];
            this.KEY_T = mappingData["KEY_T"];
            this.KEY_U = mappingData["KEY_U"];
            this.KEY_V = mappingData["KEY_V"];
            this.KEY_W = mappingData["KEY_W"];
            this.KEY_X = mappingData["KEY_X"];
            this.KEY_Y = mappingData["KEY_Y"];
            this.KEY_Z = mappingData["KEY_Z"];
            this.KEY_0 = mappingData["KEY_0"];
            this.KEY_1 = mappingData["KEY_1"];
            this.KEY_2 = mappingData["KEY_2"];
            this.KEY_3 = mappingData["KEY_3"];
            this.KEY_4 = mappingData["KEY_4"];
            this.KEY_5 = mappingData["KEY_5"];
            this.KEY_6 = mappingData["KEY_6"];
            this.KEY_7 = mappingData["KEY_7"];
            this.KEY_8 = mappingData["KEY_8"];
            this.KEY_9 = mappingData["KEY_9"];
            this.KEY_EQUALS = mappingData["KEY_EQUALS"];
            this.KEY_MINUS = mappingData["KEY_MINUS"];
            this.KEY_BACKSPACE = mappingData["KEY_BACKSPACE"];
            this.KEY_SPACEBAR = mappingData["KEY_SPACEBAR"];
            this.KEY_ENTER = mappingData["KEY_ENTER"];
            this.KEY_UP = mappingData["KEY_UP"];
            this.KEY_DOWN = mappingData["KEY_DOWN"];
            this.KEY_LEFT = mappingData["KEY_LEFT"];
            this.KEY_RIGHT = mappingData["KEY_RIGHT"];
            this.KEY_TAB = mappingData["KEY_TAB"];
            this.KEY_INSERT = mappingData["KEY_INSERT"];
            this.KEY_DELETE = mappingData["KEY_DELETE"];
            this.KEY_HOME = mappingData["KEY_HOME"];
            this.KEY_END = mappingData["KEY_END"];
            this.KEY_PAGE_UP = mappingData["KEY_PAGE_UP"];
            this.KEY_PAGE_DOWN = mappingData["KEY_PAGE_DOWN"];
            this.KEY_SHIFT = mappingData["KEY_SHIFT"];
            this.KEY_CTRL = mappingData["KEY_CTRL"];
            this.KEY_ALT = mappingData["KEY_ALT"];
            this.KEY_ESCAPE = mappingData["KEY_ESCAPE"];
            this.KEY_TILDE = mappingData["KEY_TILDE"];
            this.KEY_QUOTE = mappingData["KEY_QUOTE"];
            this.KEY_SEMICOLON = mappingData["KEY_SEMICOLON"];
            this.KEY_COMMA = mappingData["KEY_COMMA"];
            this.KEY_PERIOD = mappingData["KEY_PERIOD"];
            this.KEY_SLASH = mappingData["KEY_SLASH"];
            this.KEY_BRACKET_LEFT = mappingData["KEY_BRACKET_LEFT"];
            this.KEY_BRACKET_RIGHT = mappingData["KEY_BRACKET_RIGHT"];
            this.KEY_BACKSLASH = mappingData["KEY_BACKSLASH"];
            this.KEY_RIGHT_ALT = mappingData["KEY_RIGHT_ALT"];
            this.KEY_RIGHT_CTRL = mappingData["KEY_RIGHT_CTRL"];
            this.KEY_RIGHT_SHIFT = mappingData["KEY_RIGHT_SHIFT"];
            this.KEY_NUMPAD_0 = mappingData["KEY_NUMPAD_0"];
            this.KEY_NUMPAD_1 = mappingData["KEY_NUMPAD_1"];
            this.KEY_NUMPAD_2 = mappingData["KEY_NUMPAD_2"];
            this.KEY_NUMPAD_3 = mappingData["KEY_NUMPAD_3"];
            this.KEY_NUMPAD_4 = mappingData["KEY_NUMPAD_4"];
            this.KEY_NUMPAD_5 = mappingData["KEY_NUMPAD_5"];
            this.KEY_NUMPAD_6 = mappingData["KEY_NUMPAD_6"];
            this.KEY_NUMPAD_7 = mappingData["KEY_NUMPAD_7"];
            this.KEY_NUMPAD_8 = mappingData["KEY_NUMPAD_8"];
            this.KEY_NUMPAD_9 = mappingData["KEY_NUMPAD_9"];
            this.KEY_NUMPAD_PERIOD = mappingData["KEY_NUMPAD_PERIOD"];
            this.KEY_NUMPAD_PLUS = mappingData["KEY_NUMPAD_PLUS"];
            this.KEY_NUMPAD_MINUS = mappingData["KEY_NUMPAD_MINUS"];
            this.KEY_NUMPAD_ENTER = mappingData["KEY_NUMPAD_ENTER"];
            this.KEY_F_1 = mappingData["KEY_F_1"];
            this.KEY_F_2 = mappingData["KEY_F_2"];
            this.KEY_F_3 = mappingData["KEY_F_3"];
            this.KEY_F_4 = mappingData["KEY_F_4"];
            this.KEY_F_5 = mappingData["KEY_F_5"];
            this.KEY_F_6 = mappingData["KEY_F_6"];
            this.KEY_F_7 = mappingData["KEY_F_7"];
            this.KEY_F_8 = mappingData["KEY_F_8"];
            this.KEY_F_9 = mappingData["KEY_F_9"];
            this.KEY_F_10 = mappingData["KEY_F_10"];
            this.KEY_F_11 = mappingData["KEY_F_11"];
            this.KEY_F_12 = mappingData["KEY_F_12"];
            this.MOUSE_Y_DOWN = mappingData["MOUSE_Y_DOWN"];
            this.MOUSE_Y_UP = mappingData["MOUSE_Y_UP"];
            this.MOUSE_X_LEFT = mappingData["MOUSE_X_LEFT"];
            this.MOUSE_X_RIGHT = mappingData["MOUSE_X_RIGHT"];
            this.MOUSE_LCLICK = mappingData["MOUSE_LCLICK"];
            this.MOUSE_RCLICK = mappingData["MOUSE_RCLICK"];
            this.MOUSE_MCLICK = mappingData["MOUSE_MCLICK"];
            this.MOUSE_WHEELUP = mappingData["MOUSE_WHEELUP"];
            this.MOUSE_WHEELDOWN = mappingData["MOUSE_WHEELDOWN"];
        }

        public string this[string key]
        {
            get { return this.mappingData[key]; }
        }
    }
}
