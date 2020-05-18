using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal static class SFOReaderUtilities
    {
        /// <summary>
        /// Converts a byte[] to a string.
        /// </summary>
        public static string ByteArrayToString(byte[] byteArray)
        {
            return SFOReaderUtilities.ByteArrayToString(byteArray, false);
        }

        public static string ByteArrayToString(byte[] byteArray, bool isUtf)
        {
            if (isUtf)
            {
                return Encoding.UTF8.GetString(byteArray);
            }
            else
            {
                return Encoding.ASCII.GetString(byteArray);
            }
        }

        /// <summary>
        /// Reverse any byte[]-Array and converts it then to an int
        /// </summary>
        public static int ByteArrayReverseToInt(byte[] b)
        {
            byte[] bTemp = new byte[b.Length];

            for (int i = b.Length - 1, j = 0; i >= 0; i--, j++)
            {
                bTemp[j] = b[i];
            }

            return SFOReaderUtilities.ByteArrayToInt(bTemp);
        }

        /// <summary>
        /// Returns any byte[] as an int
        /// </summary>
        public static int ByteArrayToInt(byte[] b)
        {
            return SFOReaderUtilities.ByteArrayToInt(b, 0);
        }

        /// <summary>
        /// Returns any byte[] as an int from the given offset
        /// </summary>
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
