using System.Collections.Generic;

namespace PluggableShell.Sdk
{
    public interface IShell
    {
        public IEnumerable<IPlugin> GetPlugins();
        public void Write(string text);
        public void Write(char text);
        public void WriteLine(string text);
    }
}
