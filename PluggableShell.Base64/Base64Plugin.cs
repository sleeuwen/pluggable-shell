using System;
using System.Text;
using PluggableShell.Sdk;

namespace PluggableShell.Base64
{
    /**
     * Plugin that will return the base64 value of the given input
     * <br/><br/>
     *
     * Usage:
     *     base64 &lt;input&gt;
     */
    public class Base64Plugin : IPlugin
    {
        public string Name => "base64";
        public string Help => "Return the base64 value of the input";

        public void Execute(IShell shell, string input)
        {
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            shell.WriteLine(base64);
        }
    }
}
