using CLI.Framework.CommandDispatcher.Attributes;
using CLI.Framework.CommandDispatcher.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Framework.Startup
{
    [CommandName("arg", Alias = "a")]
    public class TestArgs : ConsoleCommand
    {

        [ExecutionMethod]
        public void Run(bool param, int number)
        {
            Console.WriteLine("Success!");
        }
    }
}
