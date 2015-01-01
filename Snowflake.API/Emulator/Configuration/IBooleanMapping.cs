using System;
namespace Snowflake.Emulator.Configuration
{
    public interface IBooleanMapping
    {
        string False { get; }
        string FromBool(bool value);
        string True { get; }
    }
}
