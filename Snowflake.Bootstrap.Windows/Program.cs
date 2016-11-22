using Snowflake.Shell.Windows;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting Shell...");
        var snowflakeShell = new SnowflakeShell();
        snowflakeShell.StartShell();
        Console.ReadLine();
    }
}