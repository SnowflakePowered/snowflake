using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
namespace Snowflake.Core.EventDelegate
{
    public class JsonRPCEventDelegate
    {
        private string RPCUrl;
        public JsonRPCEventDelegate(int port)
        {
            this.RPCUrl = "http://localhost:" + port.ToString() + @"/";
        }

        public WebResponse InvokeMethod(string method, string methodParams, string id)
        {
            WebRequest request = WebRequest.Create(this.RPCUrl);
            request.ContentType = "application/json-rpc";
            request.Method = "POST";
            var values = new Dictionary<string, dynamic>(){
                    {"method", method},
                    {"params", methodParams},
                    {"id", id}
            };
            var data = JsonConvert.SerializeObject(values);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Flush();
            dataStream.Close();
            return request.GetResponse();
        }


        public void Notify(string eventName, dynamic eventData)
        {
            var response = this.InvokeMethod(
                "notify",
                JsonConvert.SerializeObject(
                         new Dictionary<string, dynamic>(){
                            {"eventData",  eventData},
                            {"eventName",  eventName}
                        }),
                 "null"
                );
        }
    }
}

    
