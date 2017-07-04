using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.AssemblyModulePacker
{
    internal class StateMachine
    {
        public (Func<Task<dynamic>> action, string stateMessage) PreviousState { get; private set; } 
        public (Func<Task<dynamic>> action, string stateMessage) CurrentState { get; private set; }
        public static Func<Task<dynamic>> NoOp => () => { return Task.Run<dynamic>(() => ""); };
        public StateMachine()
        {
            this.PreviousState = (StateMachine.NoOp, "Init");
        }

        public async Task<T> Transition<T>(Func<Task<dynamic>> action, string stateMessage)
        {
            this.PreviousState = this.CurrentState;
            this.CurrentState = (action, stateMessage);
            Console.WriteLine(stateMessage);
            return await this.CurrentState.action();

        }
        public async Task Transition(string stateMessage) => await this.Transition<dynamic>(StateMachine.NoOp, stateMessage);
        public async Task ExitWithState(Func<Task<dynamic>> action, string stateMessage, int exitCode = 0)
        {
            await this.Transition<dynamic>(action, stateMessage);
            Environment.Exit(exitCode);
        }
        public async Task ExitWithState(string stateMessage, int exitCode = 0) =>
            await this.ExitWithState(StateMachine.NoOp, stateMessage, exitCode);
    }
}
