using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace PluggableShell
{
    public class CollectibleAssemblyContext : AssemblyLoadContext
    {
        private readonly string basePath;

        public CollectibleAssemblyContext(string basePath)
            : base(isCollectible: true)
        {
            this.basePath = basePath;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            Console.WriteLine("Loading: " + assemblyName.Name);
            if (assemblyName.Name == "PluggableShell.Sdk")
                return AssemblyLoadContext.Default.Assemblies.FirstOrDefault(x => x.GetName().Name == "PluggableShell.Sdk");

            if (!assemblyName.Name.StartsWith("System") &&
                File.Exists(Path.Combine(basePath, assemblyName.Name + ".dll")))
                return this.LoadFromAssemblyPath(Path.Combine(basePath, assemblyName.Name + ".dll"));

            return null;
        }
    }
}
