using CLI.Framework.CommandDispatcher.Command;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CLI.Framework.CommandDispatcher.Attributes;

namespace CLI.Framework.CommandDispatcher
{
    public static class CmdDispatcher
    {

        public static void DispatchCommands(IEnumerable<ConsoleCommand> commands, string[] arguments)
        {
            foreach (var arg in arguments)
            {
                var sanitized = arg.Replace("-", "");
                var splitted = sanitized.Split('=');
                var commandName = splitted[0];
                var additionalArgs = splitted.Length == 2 ? splitted[1].Trim('"').TrimEnd('"') : null;

                var cmd = SearchCommand(commands, commandName);
                if (cmd == null) throw new Exception("No matching command found");

                DispatchCommand(cmd, additionalArgs);
            }
        }

        public static void DispatchCommand(ConsoleCommand command, string arguments)
        {
            var cmdType = command.GetType();
            var executionMethod = cmdType.GetMethods().FirstOrDefault(m => m.GetCustomAttribute<ExecutionMethod>() != null);

            if (executionMethod == null)
                throw new Exception("The command has no ExecutionMethod!");

            var methodParams = executionMethod.GetParameters();
            var parsedArgs = !string.IsNullOrEmpty(arguments) ? arguments.Split(',') : new string[0];
            if (parsedArgs.Length != methodParams.Length)
                throw new Exception("The number of arguments does not match the method definition!");

            var paramsList = new List<object>();
            for (int i = 0; i < methodParams.Length;  i++)
            {
                var param = methodParams[i];
                var arg = parsedArgs[i];
                var converted = Convert.ChangeType(arg, param.ParameterType);
                paramsList.Add(converted);
            }

            executionMethod.Invoke(command, paramsList.ToArray());
        }

        public static IEnumerable<ConsoleCommand> FindCommandsInAllLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(FindCommandsInAssembly);
        }

        public static IEnumerable<ConsoleCommand> FindCommandsInSameAssemblyAs(Type typeInSameAssembly)
        {
            if (typeInSameAssembly == null) throw new ArgumentNullException("typeInSameAssembly");
            return FindCommandsInAssembly(typeInSameAssembly.Assembly);
        }

        public static IEnumerable<ConsoleCommand> FindCommandsInAssembly(Assembly assemby)
        {
            if (assemby == null) throw new ArgumentNullException("assembly");

            var commandTypes = assemby.GetTypes().Where(t => t.IsSubclassOf(typeof(ConsoleCommand)) && !t.IsAbstract);

            List<ConsoleCommand> result = new List<ConsoleCommand>();
            foreach (var commandType in commandTypes)
            {
                var ctor = commandType.GetConstructor(new Type[] { });
                result.Add((ConsoleCommand)ctor.Invoke(new object[] { }));
            }

            

            return result;
        }

        private static ConsoleCommand SearchCommand(IEnumerable<ConsoleCommand> commands, string name)
        {
            ConsoleCommand result = null;
            foreach (var cmd in commands)
            {
                var attribute = cmd.GetType().GetCustomAttribute<CommandName>();
                if(attribute.Name == name || attribute.Alias == name)
                {
                    result = cmd;
                    break;
                }
            }

            return result;
        }
    }
}
