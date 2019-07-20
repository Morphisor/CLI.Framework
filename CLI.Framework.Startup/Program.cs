using CLI.Framework.CommandDispatcher;
using System;

namespace CLI.Framework.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = CmdDispatcher.FindCommandsInAllLoadedAssemblies();
            CmdDispatcher.DispatchCommands(commands, args);
        }
    }
}
