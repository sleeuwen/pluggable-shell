using System;
using System.Text.RegularExpressions;
using PluggableShell.Sdk;

namespace PluggableShell.RegexMatch
{
    /**
     * A simple plugin that can display regex matches for a given string.
     * <br/><br/>
     * Usage:
     *     matches &lt;regex&gt; &lt;input&gt;
     */
    public class RegexMatchPlugin : IPlugin
    {
        public string Name => "matches";
        public string Help => "Check if a given string matches the regex\n\nUsage: matches [regex] [string]";
        public void Execute(IShell shell, string input)
        {
            var args = input?.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (args == null || args.Length != 2)
            {
                shell.WriteLine("Invalid number of arguments");
                return;
            }

            Regex re;
            try
            {
                re = new Regex(args[0]);
            }
            catch (Exception ex)
            {
                shell.WriteLine("Unable to parse regex: " + ex.Message);
                return;
            }

            var matches = re.Matches(args[1]);
            shell.WriteLine("# of matches: " + matches.Count);
            shell.WriteLine("");
            foreach (var match in matches)
            {
                shell.WriteLine(Convert.ToString(match));
            }
        }
    }
}
