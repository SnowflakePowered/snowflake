using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Selections.AudioConfiguration
{
    public enum AspectRatioIndex
    {
        [SelectionOption("21", DisplayName = "Automatic")]
        CoreProvided,
        [SelectionOption("0", DisplayName = "4:3")]
        FourThree,
        [SelectionOption("1", DisplayName = "16:9")]
        SixteenNine,
        [SelectionOption("2", DisplayName = "16:10")]
        SixteenTen,
        [SelectionOption("3", DisplayName = "16:15")]
        SixteenFifteen,
        [SelectionOption("4", DisplayName = "1:1")]
        OneOne,
        [SelectionOption("5", DisplayName = "2:1")]
        TwoOne,
        [SelectionOption("6", DisplayName = "3:2")]
        ThreeTwo,
        [SelectionOption("7", DisplayName = "3:4")]
        ThreeFour,
        [SelectionOption("8", DisplayName = "4:1")]
        FourOne,
        [SelectionOption("9", DisplayName = "4:4")]
        FourFour,
        [SelectionOption("10", DisplayName = "5:4")]
        FiveFour,
        [SelectionOption("11", DisplayName = "6:5")]
        SixFive,
        [SelectionOption("12", DisplayName = "7:9")]
        SevenNine,
        [SelectionOption("13", DisplayName = "8:3")]
        EightThree,
        [SelectionOption("14", DisplayName = "8:7")]
        EightSeven,
        [SelectionOption("15", DisplayName = "19:12")]
        NineteenTwelve,
        [SelectionOption("16", DisplayName = "19:14")]
        NineteenFourteen,
        [SelectionOption("17", DisplayName = "30:17")]
        ThirtySeventeen,
        [SelectionOption("18", DisplayName = "32:9")]
        ThirtytwoNine,
        [SelectionOption("20", DisplayName = "4:3 (Square Pixels)")]
        FourThreeSquarePixels
    }
}
