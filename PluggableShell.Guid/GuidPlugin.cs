using PluggableShell.Sdk;

namespace PluggableShell.Guid
{
    /**
     * Plugin that will print a random guid
     * <br/><br/>
     * Usage:
     *     guid
     */
    public class GuidPlugin : IPlugin
    {
        public string Name => "guid";
        public string Help => "Get a random guid";
        public void Execute(IShell shell, string input)
        {
            shell.WriteLine(System.Guid.NewGuid().ToString());
        }
    }
}
