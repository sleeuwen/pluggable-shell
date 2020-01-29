using System;
using Newtonsoft.Json;
using PluggableShell.Sdk;

namespace PluggableShell.Json
{
    public class JsonPlugin : IPlugin
    {
        public string Name => "json";
        public string Help => "Pretty print json";

        public void Execute(IShell shell, string input)
        {
            try
            {
                var json = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(input), Formatting.Indented);
                shell.WriteLine(json);
            }
            catch (JsonException ex)
            {
                shell.WriteLine("Input was not valid JSON");
            }
        }
    }
}
