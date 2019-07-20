using CLI.Framework.CommandDispatcher.Attributes;
using System;
using System.Reflection;
using System.Linq;

namespace CLI.Framework.CommandDispatcher.Command
{
    [CommandName("help", Alias = "h")]
    public class HelpCommand : ConsoleCommand
    {

        [ExecutionMethod]
        public void PrintHelp()
        {
            var commands = CmdDispatcher.FindCommandsInAllLoadedAssemblies().Where(c => c.GetType() != typeof(HelpCommand));
            foreach (var cmd in commands)
            {
                var cmdType = cmd.GetType();
                var attribute = cmdType.GetCustomAttribute<CommandName>();
                var executionMethod = cmdType.GetMethods().FirstOrDefault(m => m.GetCustomAttribute<ExecutionMethod>() != null);
                var methodParams = executionMethod.GetParameters();
                var paramsDescription = string.Join(", ", methodParams.Select(p => p.Name));

                Console.WriteLine($"Name: {attribute.Name}, Alias: {attribute.Alias}, Description: {attribute.Description}, args: {paramsDescription}");
            }
        }

    }
}
