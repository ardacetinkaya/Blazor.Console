namespace Blazor.Console
{
    using Blazor.Console.Command;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsole(this IServiceCollection serviceCollection,Action<ConsoleOptions> options=null)
        {

            serviceCollection.AddTransient<ICommandRunning, CommandRunning>();
            var consoleOptions = new ConsoleOptions();

            if (options != null)
            {
            
                options.Invoke(consoleOptions);
            }

            serviceCollection.AddTransient<ILongRunningCommand, LongCommand>();
            if (consoleOptions.UseDefaultServices)
            {
                serviceCollection.AddTransient<IHelpCommand, HelpCommand>();
                serviceCollection.AddTransient<IOSCommand, OSCommand>();
                serviceCollection.AddTransient<IVersionCommand, VersionCommand>();
            }

            return serviceCollection;
        }


        public class ConsoleOptions
        {
            public bool UseDefaultServices { get; set; } = true;
        }
    }
}
