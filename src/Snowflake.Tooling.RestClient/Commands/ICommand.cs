using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient.Commands
{
    public interface ICommand<TCommandOptions>
    {
        Task<int> Execute(TCommandOptions options);
    }
}
