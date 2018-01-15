using System;
using Snowflake.Shell.Windows;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting Shell...");
        var snowflakeShell = new SnowflakeShell();
        while (Console.ReadLine() != "exit") ;
    }
}