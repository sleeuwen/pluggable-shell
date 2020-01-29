using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PluggableShell.Sdk;

namespace PluggableShell
{
    class PluggableShell : IShell
    {
        private readonly List<IPlugin> plugins;

        public PluggableShell(IEnumerable<IPlugin> plugins)
        {
            this.plugins = plugins.ToList();
        }

        public void Run()
        {
            Console.WriteLine("Available commands:");
            foreach (var plugin in plugins)
                Console.WriteLine(plugin.Name);

            Console.WriteLine("Type help <name> to get help for a specific command");
            Console.WriteLine("Type quit or exit to exit the shell");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> ");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    continue;

                var command = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if ("quit".Equals(command[0], StringComparison.OrdinalIgnoreCase) || "exit".Equals(command[0], StringComparison.OrdinalIgnoreCase))
                    break;

                var handler = FindHandler(command[0]);
                if (handler == null)
                {
                    Console.WriteLine("Invalid command: " + command[0]);
                    Console.WriteLine("Type help for a list of available commands");
                }
                else
                {
                    try
                    {
                        handler.Execute(this, command.Length > 1 ? command[1] : null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandled error executing command: " + ex.Message);
                    }
                }
            }
        }

        private IPlugin FindHandler(string command)
        {
            foreach (var plugin in plugins)
            {
                if (plugin.Name.Equals(command, StringComparison.OrdinalIgnoreCase))
                    return plugin;
            }

            return null;
        }

        public IEnumerable<IPlugin> GetPlugins()
        {
            return plugins;
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
