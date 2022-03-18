using System;
using Snowflake.Shell.Windows;

class Program
{
    private static SnowflakeShell snowflakeShell;

    static void Main(string[] args)
    {
        Console.WriteLine("Starting Shell...");
        AppDomain.CurrentDomain.ProcessExit += ExitHandler;
        Program.snowflakeShell = new SnowflakeShell();
        snowflakeShell.StartCore();
        while (Console.ReadLine() != "exit") ;

        Console.WriteLine("Shutting down...");
        snowflakeShell.ShutdownCore();

    }

    private static void ExitHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Shutting down due to force exit...");
        if (!snowflakeShell.IsShutdown)
        {
            snowflakeShell.ShutdownCore();
        }
    }
}
