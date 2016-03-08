using System;
using System.Text;

namespace Snowflake.Utility
{
    public static class StringEncode
    {
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        public static string atob(string a)
        {
            return StringEncode.EncodeTo64(a);
        }
        public static string btoa(string b)
        {
            return StringEncode.DecodeFrom64(b);
        }

    }
}
