using CLI.Framework.CommandDispatcher;
using CLI.Framework.Gui;
using System;

namespace CLI.Framework.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.Start();
            var commands = CmdDispatcher.FindCommandsInAllLoadedAssemblies();
            CmdDispatcher.DispatchCommands(commands, args);
        }
    }
}
