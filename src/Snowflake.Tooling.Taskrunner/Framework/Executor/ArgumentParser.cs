using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Framework.Parser
{
    public sealed class ArgumentParser
    {
        public IDictionary<int, string> ParsePositionalArguments(string[] args)
        {
            Dictionary<int, string> parsedArgs = new Dictionary<int, string>();

            int counter = -1;
            for (int i = 0; i < args.Length; i++)
            {
                string currentArg = args[i];
                if (!currentArg.StartsWith("-"))
                {
                    parsedArgs.Add(++counter, currentArg);
                }
                else
                {
                    i++; // skip the next argument as well.
                }
            }
            return parsedArgs;
        }

        public Dictionary<string, string> ParseNamedArguments(string[] args)
        {
            Dictionary<string, string> parsedArgs = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                string currentArg = args[i];
                if (currentArg.StartsWith("--"))
                {
                    string option = currentArg.Substring(2);
                    if (i+1 >= args.Length || args[i + 1].StartsWith("-"))
                    {
                        parsedArgs.Add(option, "true"); // switch at the end, or a switch value.
                        continue;
                    }
                    parsedArgs.Add(option, args[++i]);
                }
                else if (currentArg.StartsWith("-"))
                {
                    string option = currentArg.Substring(1);
                    if (i + 1 >= args.Length || args[i + 1].StartsWith("-"))
                    {
                        parsedArgs.Add(option, "true"); // switch at the end, or a switch value.
                        continue;
                    }
                    parsedArgs.Add(option, args[++i]);
                }
            }
            return parsedArgs;
        }       
    }
}
