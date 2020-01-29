using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace PluggableShell
{
    /**
     * Custom assembly load context that is collectible and knows where to load plugin assemblies from.
     *
     * Used for looking up plugin assemblies and loading them.
     */
    public class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string basePath;

        public PluginAssemblyLoadContext(string basePath)
            : base(isCollectible: true)
        {
            this.basePath = basePath;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            
            if (assemblyName.Name == "PluggableShell.Sdk")
                return AssemblyLoadContext.Default.Assemblies.FirstOrDefault(x => x.GetName().Name == "PluggableShell.Sdk");

            if (!assemblyName.Name.StartsWith("System") &&
                File.Exists(Path.Combine(basePath, assemblyName.Name + ".dll")))
                return this.LoadFromAssemblyPath(Path.Combine(basePath, assemblyName.Name + ".dll"));

            return null;
        }
    }
}
