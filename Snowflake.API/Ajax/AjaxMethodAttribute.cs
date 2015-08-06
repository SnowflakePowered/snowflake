using System;

namespace Snowflake.Ajax
{
    /// <summary>
    /// Methods that will be exported in a AjaxNamespace plugin should be marked with this attribute
    /// Only methods in a <see cref="Snowflake.Ajax.IBaseAjaxNamespace"/> will be callable via Ajax
    /// <remarks>
    /// <seealso cref="Snowflake.Ajax.BaseAjaxNamespace"/> for the implementation of <seealso cref="Snowflake.Ajax.IBaseAjaxNamespace"/>
    /// </remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AjaxMethodAttribute : Attribute
    {
        /// <summary>
        /// The name of the AJAX Method
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// The prefix of the AJAX Method
        /// This prefix is essentially a sub-namespace within the AjaxNameSpace of your Ajax plugin
        /// <example>Game.GetAllGames, where Game is the MethodPrefix</example>
        /// </summary>
        public string MethodPrefix { get; set; }
      
    }
}