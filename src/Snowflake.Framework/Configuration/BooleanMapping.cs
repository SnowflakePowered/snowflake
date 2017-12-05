
namespace Snowflake.Configuration
{
    public class BooleanMapping : IBooleanMapping
    {
        /// <inheritdoc/>
        public string True { get; }

        /// <inheritdoc/>
        public string False { get; }

        public BooleanMapping(string trueValue, string falseValue)
        {
            this.True = trueValue;
            this.False = falseValue;
        }

        /// <inheritdoc/>
        public string FromBool(bool value)
        {
            return value ? this.True : this.False;
        }

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "True"
        /// False: "False
        /// </summary>
        public static IBooleanMapping TitlecaseBooleanMapping => new BooleanMapping("True", "False");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "true"
        /// False: "false"
        /// </summary>
        public static IBooleanMapping LowercaseBooleanMapping => new BooleanMapping("true", "false");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "TRUE"
        /// False: "FALSE"
        /// </summary>
        public static IBooleanMapping UppercaseBooleanMapping => new BooleanMapping("TRUE", "FALSE");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "1"
        /// False : "0"
        /// </summary>
        public static IBooleanMapping BinaryBooleanMapping => new BooleanMapping("1", "0");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "Yes"
        /// False: "No"
        /// </summary>
        public static IBooleanMapping TitlecaseYesNoBooleanMapping => new BooleanMapping("Yes", "No");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "yes"
        /// False: "no"
        /// </summary>
        public static IBooleanMapping LowercaseYesNoBooleanMapping => new BooleanMapping("yes", "no");

        /// <summary>
        /// Gets boolean mapping where
        ///
        /// True: "YES"
        /// False: "NO"
        /// </summary>
        public static IBooleanMapping UppercaseYesNoBooleanMapping => new BooleanMapping("YES", "NO");
    }
}
