using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Snowflake.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        internal SnowflakeShell()
        {
#if DEBUG
            CoreService.InitCore(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
#else
            CoreService.InitCore();
#endif
            
            CoreService.InitPluginManager();

         /*   Snowflake.Events.SnowflakeEventSource.EventSource.AjaxRequestReceived += (s, e) =>
            {
                Console.WriteLine
                (
                     Regex.Replace
                     (
                     "Received Request " +
                     e.ReceivedRequest.MethodName +
                     " " +
                     JsonConvert.SerializeObject(e.ReceivedRequest.MethodParameters) +
                     Environment.NewLine
                     , "(?<!\r)\n", "\r\n"
                     )
                 );
            };
            Snowflake.Events.SnowflakeEventSource.EventSource.AjaxResponseSending += (s, e) =>
            {
                Console.WriteLine
                (
                    Regex.Replace
                    (
                     "Sending Response " +
                     e.SendingResponse.GetJson() +
                     Environment.NewLine
                     , "(?<!\r)\n", "\r\n"
                     )
                 );
            };*/
        }

        public void RestartCore()
        {
            this.ShutdownCore();
#if DEBUG
            CoreService.InitCore(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            CoreService.InitPluginManager();
            Console.WriteLine("Core Service Restarted");
#else
            CoreService.InitCore();
            CoreService.InitPluginManager();
#endif
        }

        public void ShutdownCore()
        {
            CoreService.DisposeLoadedCore();
            GC.WaitForPendingFinalizers();
        }
    }
}
