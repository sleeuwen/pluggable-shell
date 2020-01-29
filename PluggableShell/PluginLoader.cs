using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using PluggableShell.Sdk;

namespace PluggableShell
{
    internal static class PluginLoader
    {
        public static IEnumerable<string> FindPluginAssemblies(string directory)
        {
            var files = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                // Don't check the sdk for applicable types as we know there aren't any.
                if (file.EndsWith("PluggableShell.Sdk.dll"))
                    continue;

                if (IsPluginAssembly(Path.Combine(directory, file)))
                    yield return Path.Combine(directory, file);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsPluginAssembly(string location)
        {
            var loadContext = new PluginAssemblyLoadContext(Path.GetDirectoryName(location));
            loadContext.LoadFromAssemblyPath(location);
            var containsPlugins = loadContext.Assemblies
                .SelectMany(x => x.ExportedTypes)
                .Any(t => t.IsClass && !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));

            loadContext.Unload();
            loadContext = null;

            return containsPlugins;
        }
    }
}
