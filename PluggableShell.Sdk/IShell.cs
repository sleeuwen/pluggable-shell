using System.Collections.Generic;

namespace PluggableShell.Sdk
{
    public interface IShell
    {
        /**
         * <summary>Get a list of all available plugins in the current shell</summary>
         */
        public IEnumerable<IPlugin> GetPlugins();
        
        /**
         * <summary>Write text to the shell</summary>
         */
        public void Write(string text);
        
        /**
         * <summary>Write text to the shell followed by a newline</summary>
         */
        public void WriteLine(string text);
    }
}
