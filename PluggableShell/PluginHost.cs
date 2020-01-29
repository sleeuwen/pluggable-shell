using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PluggableShell.Sdk;

namespace PluggableShell
{
    internal class PluginHost : IDisposable
    {
        private readonly PluginAssemblyLoadContext loadContext;

        public List<Type> PluginTypes { get; }

        private PluginHost(PluginAssemblyLoadContext loadContext)
        {
            this.loadContext = loadContext;

            PluginTypes = loadContext.Assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IPlugin).IsAssignableFrom(type))
                .ToList();
        }

        public static PluginHost Load(string assembly)
        {
            var context = new PluginAssemblyLoadContext(Path.GetDirectoryName(assembly));
            context.LoadFromAssemblyPath(assembly);

            return new PluginHost(context);
        }

        public void Dispose()
        {
            PluginTypes.Clear();
            loadContext.Unload();
        }
    }
}
