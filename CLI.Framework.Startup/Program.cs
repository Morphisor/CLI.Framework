using CLI.Framework.CommandDispatcher;
using System;

namespace CLI.Framework.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = CmdDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            CmdDispatcher.DispatchCommands(commands, args);
        }
    }
}
