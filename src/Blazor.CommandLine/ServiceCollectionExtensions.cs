namespace Blazor.Components
{
    using Blazor.Components.CommandLine;
    using Blazor.Components.CommandLine.Command;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandLine(this IServiceCollection serviceCollection,Action<CommandLineOptions> options=null)
        {

            serviceCollection.AddTransient<IRunningCommand, RunningCommand>();
            var consoleOptions = new CommandLineOptions();

            if (options != null)
            {
            
                options.Invoke(consoleOptions);
            }
            
            if (consoleOptions.UseDefaultServices)
            {
                serviceCollection.AddTransient<IHelpCommand, HelpCommand>();
                serviceCollection.AddTransient<IOSCommand, OSCommand>();
                serviceCollection.AddTransient<IVersionCommand, VersionCommand>();
            }

            return serviceCollection;
        }

        public class CommandLineOptions
        {
            public bool UseDefaultServices { get; set; } = true;
        }
    }
}
