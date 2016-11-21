namespace Snowflake.Ajax
{
    /// <summary>
    /// Represents a Javascript (JSON) response to an AJAX request
    /// </summary>
    public interface IJSResponse
    {
        /// <summary>
        /// Gets a string representation of the generated JSON with the provided response payload
        /// </summary>
        /// <returns>The generated JSON string</returns>
        string GetJson();
        /// <summary>
        /// The payload object to be returned to the request
        /// <remarks>The payload will be serialized to JSON before sent</remarks>
        /// </summary>
        dynamic Payload { get; }
        /// <summary>
        /// The AJAX request that prompted this response
        /// </summary>
        IJSRequest Request { get; }
        /// <summary>
        /// Whether the request was successful
        /// </summary>
        bool Success { get; }
    }
}
