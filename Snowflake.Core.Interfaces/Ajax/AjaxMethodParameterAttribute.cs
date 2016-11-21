using System;

namespace Snowflake.Ajax
{
    /// <summary>
    /// A metadata attribute to indicate parameter methods
    /// Does not affect execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class AjaxMethodParameterAttribute : Attribute
    {
        //The name of the parameter
        public string ParameterName { get; set; }
        /// <summary>
        /// The type of parameter
        /// </summary>
        public AjaxMethodParameterType ParameterType { get; set; }
        /// <summary>
        /// Whether the parameter is required
        /// </summary>
        public bool Required { get; set; }
    }
    public enum AjaxMethodParameterType
    {
        StringParameter,
        ObjectParameter,
        ArrayParameter,
        BoolParameter,
        IntParameter
    }
}
