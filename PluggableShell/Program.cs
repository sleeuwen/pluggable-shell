using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PluggableShell.Sdk;

namespace PluggableShell
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<PluggableShell>();
            services.AddTransient<IPlugin, HelpPlugin>();
            services.AddLogging(logging => logging
                    .AddConsole()
#if DEBUG
                    .SetMinimumLevel(LogLevel.Debug)
#else
                    .SetMinimumLevel(LogLevel.Information)
#endif
            );

            var logger = services.BuildServiceProvider().GetRequiredService<ILogger>();

            var pluginDirectory = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Plugins");
            logger.LogDebug("Loading plugins from {directory}", pluginDirectory);
            foreach (var assembly in PluginLoader.FindPluginAssemblies(pluginDirectory))
            {
                logger.LogDebug("Found plugin assembly {assembly}", assembly);

                var pluginHost = PluginHost.Load(assembly);
                services.AddSingleton(pluginHost);

                foreach (var pluginType in pluginHost.PluginTypes)
                    services.AddTransient(_ => (IPlugin) Activator.CreateInstance(pluginType));
            }

            var sp = services.BuildServiceProvider();
            var shell = sp.GetRequiredService<PluggableShell>();
            shell.Run();
        }
    }
}
