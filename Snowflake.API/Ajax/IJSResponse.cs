using System;
namespace Snowflake.Ajax
{
    public interface IJSResponse
    {
        string GetJson();
        dynamic Payload { get; }
        IJSRequest Request { get; }
    }
}
