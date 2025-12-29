using Microsoft.Extensions.DependencyInjection;
using System;
using Blazor.CommandLine.Command;

namespace Blazor.CommandLine;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLine(this IServiceCollection serviceCollection,
        Action<CommandLineOptions> options = null)
    {
        serviceCollection.AddTransient<IRunningCommand, RunningCommand>();
        var consoleOptions = new CommandLineOptions();

        options?.Invoke(consoleOptions);

        return serviceCollection;
    }

    public class CommandLineOptions
    {
        public bool UseDefaultServices { get; set; } = true;
    }
}