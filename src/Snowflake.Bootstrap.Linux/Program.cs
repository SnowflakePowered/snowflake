using System;

namespace Snowflake.Shell.Linux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Shell...");
            var snowflakeShell = new SnowflakeShell();
            snowflakeShell.StartShell();
            Console.ReadLine();
        }
    }
}
