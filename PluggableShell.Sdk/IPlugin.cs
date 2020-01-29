namespace PluggableShell.Sdk
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Help { get; }

        public void Execute(IShell shell, string input);
    }
}
