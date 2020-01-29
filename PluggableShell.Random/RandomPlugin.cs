using System;
using PluggableShell.Sdk;

namespace PluggableShell.Random
{
    /**
     * Generate a random number, possibly with a minimum and maximum value
     * <br/><br/>
     * Usage:
     *     random [[min], max]
     */
    public class RandomPlugin : IPlugin
    {
        public string Name => "random";
        public string Help => "Get a random number";

        public void Execute(IShell shell, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                shell.WriteLine(Convert.ToString(new System.Random().Next()));
                return;
            }

            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 1)
            {
                var max = Convert.ToInt32(args[0]);
                shell.WriteLine(Convert.ToString(new System.Random().Next(max)));
            }
            else if (args.Length == 2)
            {
                var min = Convert.ToInt32(args[0]);
                var max = Convert.ToInt32(args[1]);
                shell.WriteLine(Convert.ToString(new System.Random().Next(min, max)));
            }
            else
            {
                shell.WriteLine("Invalid number of arguments");
            }
        }
    }
}
