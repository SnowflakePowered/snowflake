using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Ajax
{
    /// <summary>
    /// A metadata attribute to indicate parameter methods
    /// Does not affect execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class AjaxMethodParameterAttribute : Attribute
    {
        public string ParameterName { get; set; }
        public AjaxMethodParameterType ParameterType { get; set; }
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
