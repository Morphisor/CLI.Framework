using CLI.Framework.CommandDispatcher.Attributes;
using CLI.Framework.CommandDispatcher.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Framework.Startup
{
    [CommandName("test", Alias = "t")]
    public class TestCommand : ConsoleCommand
    {

        [ExecutionMethod]
        public void Run()
        {
            Console.WriteLine("Success!");
        }
    }
}
