namespace PluggableShell.Sdk
{
    public interface IPlugin
    {
        /**
         * <summary>The name of the plugin</summary>
         */
        public string Name { get; }
        
        /**
         * <summary>Help message to show the usage of this plugin</summary>
         */
        public string Help { get; }

        /**
         * <summary>Execute the plugin</summary>
         */
        public void Execute(IShell shell, string input);
    }
}
