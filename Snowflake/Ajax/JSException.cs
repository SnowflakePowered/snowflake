using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Ajax
{
    public class JSException : IJSException
    {

        public Exception FullException { get; }
        public string Message { get; }
        public string ExceptionName { get; }
        public IJSRequest SourceRequest { get; }

        public JSException(Exception fullException, IJSRequest jsRequest = null)
        {
            this.FullException = fullException;
            this.Message = this.FullException.Message;
            this.ExceptionName = this.FullException.GetType().Name;
            this.SourceRequest = jsRequest;
        }
       
    }
}
