using System;
using System.Linq;
using PluggableShell.Sdk;

namespace PluggableShell
{
    public class HelpPlugin : IPlugin
    {
        public string Name => "help";
        public string Help => "Displays the help message of a plugin";

        public void Execute(IShell shell, string input)
        {
            if (string.IsNullOrEmpty(input))
                AvailablePlugins(shell);
            else
                ShowHelpFor(shell, input);
        }

        private void AvailablePlugins(IShell shell)
        {
            shell.WriteLine("Available commands:");
            foreach (var plugin in shell.GetPlugins())
            {
                Console.WriteLine(plugin.Name);
            }
        }

        private void ShowHelpFor(IShell shell, string command)
        {
            var plugin = shell.GetPlugins().FirstOrDefault(p => p.Name.Equals(command, StringComparison.OrdinalIgnoreCase));
            if (plugin == null)
                shell.WriteLine($"Invalid command: {command}");
            else
            {
                shell.WriteLine(plugin.Name + ": " + plugin.Help);
            }
        }
    }
}
