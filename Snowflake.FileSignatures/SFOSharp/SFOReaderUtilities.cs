using System.Text;

namespace Snowflake.Plugin.FileSignatures.SFOSharp
{
    internal static class SFOReaderUtilities
    {
        /**
         * Converts any byte[]-Array to a string with the specified encoding.
         * 
         * @param byteArray
         * @param encoding
         * @return String
         */

        public static string ByteArrayToString(byte[] byteArray)
        {
            return SFOReaderUtilities.ByteArrayToString(byteArray, false);
        }

        public static string ByteArrayToString(byte[] byteArray, bool isUtf)
        {
            if (isUtf)
                return Encoding.UTF8.GetString(byteArray);
            else
                return Encoding.ASCII.GetString(byteArray);
        }

        /**
         * Reverse any byte[]-Array and converts it then to an int
         * 
         * @param b
         * @return int
         */
        public static int ByteArrayReverseToInt(byte[] b)
        {
            byte[] bTemp = new byte[b.Length];

            for (int i = b.Length - 1, j = 0; i >= 0; i--, j++)
            {
                bTemp[j] = b[i];
            }

            return SFOReaderUtilities.ByteArrayToInt(bTemp);
        }

        /**
         * Returns any byte[]-Array as an int
         * 
         * @param b
         * @return Integer
         */
        public static int ByteArrayToInt(byte[] b)
        {
            return SFOReaderUtilities.ByteArrayToInt(b, 0);
        }

        /**
         * Returns any byte[]-Array as an int from the given offset
         * 
         * @param b
         * @param offset
         * @return Integer
         */
        public static int ByteArrayToInt(byte[] b, int offset)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                int shift = (4 - 1 - i) * 8;
                value += (b[i + offset] & 0x000000FF) << shift;
            }
            return value;
        }
    }
}
