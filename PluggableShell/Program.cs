using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PluggableShell.Sdk;

namespace PluggableShell
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<ModularShell>();
            services.AddTransient<IPlugin, HelpPlugin>();

            var plugins = new List<PluginHost>();
            Console.WriteLine("Loading plugins from " + Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Plugins"));
            foreach (var assembly in PluginLoader.FindPluginAssemblies(
                Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Plugins")))
            {
                Console.WriteLine("Found plugin: " + assembly);
                plugins.Add(PluginHost.Load(assembly, services));
            }

            var sp = services.BuildServiceProvider();
            var shell = sp.GetRequiredService<ModularShell>();
            shell.Run();
        }
    }

    class ModularShell : IShell
    {
        private readonly List<IPlugin> plugins;

        public ModularShell(IEnumerable<IPlugin> plugins)
        {
            this.plugins = plugins.ToList();
        }

        public void Run()
        {
            Console.WriteLine("Available commands:");
            foreach (var plugin in plugins)
            {
                Console.WriteLine(plugin.Name);
            }

            Console.WriteLine("Type help <name> to get help for a specific command");
            Console.WriteLine("Type quit or exit to exit the shell");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> ");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    continue;

                var command = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if ("quit".Equals(command[0], StringComparison.OrdinalIgnoreCase) || "exit".Equals(command[0], StringComparison.OrdinalIgnoreCase))
                    break;

                var handler = FindHandler(command[0]);
                if (handler == null)
                {
                    Console.WriteLine("Invalid command: " + command[0]);
                    Console.WriteLine("Type help for a list of available commands");
                }
                else
                {
                    try
                    {
                        handler.Execute(this, command.Length > 1 ? command[1] : null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandled error executing command: " + ex.Message);
                    }
                }
            }
        }

        private IPlugin FindHandler(string command)
        {
            foreach (var plugin in plugins)
            {
                if (plugin.Name.Equals(command, StringComparison.OrdinalIgnoreCase))
                    return plugin;
            }

            return null;
        }

        public IEnumerable<IPlugin> GetPlugins()
        {
            return plugins;
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void Write(char text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }

    class PluginHost : IDisposable
    {
        private readonly CollectibleAssemblyContext context;
        private readonly List<Type> pluginTypes;

        private PluginHost(CollectibleAssemblyContext context)
        {
            this.context = context;

            pluginTypes = context.Assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IPlugin).IsAssignableFrom(type))
                .ToList();
        }

        public static PluginHost Load(string assembly, ServiceCollection services)
        {
            var context = new CollectibleAssemblyContext(Path.GetDirectoryName(assembly));
            context.LoadFromAssemblyPath(assembly);

            var host = new PluginHost(context);
            foreach (var type in host.pluginTypes)
                services.AddTransient(sp => (IPlugin) Activator.CreateInstance(type));

            return host;
        }

        public void Dispose()
        {
            pluginTypes.Clear();
            context.Unload();
        }
    }

    static class PluginLoader
    {
        public static IEnumerable<string> FindPluginAssemblies(string directory)
        {
            var files = Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith("PluggableShell.Sdk.dll"))
                    continue;

                if (IsPluginAssembly(Path.Combine(directory, file)))
                    yield return Path.Combine(directory, file);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsPluginAssembly(string location)
        {
            var loadContext = new CollectibleAssemblyContext(Path.GetDirectoryName(location));
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
