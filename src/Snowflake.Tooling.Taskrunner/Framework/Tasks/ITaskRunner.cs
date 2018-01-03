using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.Taskrunner.Framework.Tasks
{
    public interface ITaskRunner<TArgs> : ITaskRunner where TArgs : class, new()
    {
        Task<int> Execute(TArgs arguments, string[] args);
    }

    public interface ITaskRunner
    {
        string Name { get; }
        string Description { get; }
        Type ArgumentType { get; }
        VerbTaskResult Execute(object arguments, string[] args);
    }
}
