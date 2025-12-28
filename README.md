# Blazor.CommandLine (a.k.a Blazor.Console)

[![Actions Status](https://github.com/ardacetinkaya/Blazor.Console/workflows/Build/badge.svg)](https://github.com/ardacetinkaya/Blazor.Console/actions)
[![NuGet version (Blzr.Components.CommandLine)](https://img.shields.io/nuget/v/Blzr.Components.CommandLine.svg)](https://www.nuget.org/packages/Blzr.Components.CommandLine/)

Development: ![Build](https://github.com/ardacetinkaya/Blazor.Console/workflows/Build/badge.svg?branch=development)
<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/main/screenshots/1.png" />
</p>

### What and why... ###

This is a simple console component for ASP.NET Core 3.0 Blazor Server application model. The motivation behind this simple component is to provide simple command-line interface to manage some Web API. With this component it is easy to execute some business related commands.* 

Real life scenario examples;
- Clear response cache
- Export log files
- Change run-time settings
- Monitor application resources

This Blazor component is based on ```System.CommandLine``` API and reflects standart command-line features to be more reliable.

### Usage ###

Check Demo project(BlazorConsoleDemo.csproj) for usage;

Add following extensions to services and application within Startup.cs

```csharp

builder.Services.AddCommandLine();

app.UseCommandLine(app.Environment.WebRootPath);

```

Add <BlazorCommandLine> tag into needed view file(ex: Index.razor)

```html
@page "/"
@using Blazor.CommandLine
@using Blazor.Components.CommandLine
@using Blazor.Components.CommandLine.Console

<h1>Hello, world!</h1>

<BlazorCommandLine @ref="console" Name="Some Demo App v1.0.0"/>
```

And to have fancy UI add CSS to host file, _HOST.cshtml

```html
<link href="Blazor.CommandLine/styles.css" rel="stylesheet" />
```

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/main/screenshots/2.png" />
</p>

### Commands ###

Adding commands to Blazor.CommandLine is not complicated. First create a command class, implement it with ```BaseCommand``` . Then add the command(s) to the BlazorCommandLine's Commands as below;

Within custom command's constructor it is easy to add options to the command. Also command arguments can be enabled for the command as ```System.CommandLine```

To implement the command's main execution just override Execute() or ExecuteAsync() method. 

```cshtml
@page "/"
@using Blazor.CommandLine
@using Blazor.Components.CommandLine
@using Blazor.Components.CommandLine.Console

@inject IRunningCommand RunningCommand

<h1>Hello, world!</h1>

<BlazorCommandLine @ref="console" />

@code
{
    BlazorCommandLine _console;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        _console.Commands.Add(new CommandExample("simple", "Description of command"));
        _console.Commands.Add(new LongCommand("lng", "Description of long-running command"));

        return base.OnAfterRenderAsync(firstRender);
    }

    public class CommandExample : BaseCommand
    {
        public CommandExample(string name, string description) : base(name, description)
        {
            base.AddOption("test", "Description of option test");
            base.AddOption("file", "Description of option file");
        }

        protected override bool Execute(ConsoleOut console, params KeyValuePair<string, string>[] options)
        {
            console.Write("This is output of a simple command");
            foreach (var option in options)
            {
                console.Write(option.Value);
            }
            return true;
        }
    }

    public class LongCommand(string name, string description) : BaseCommand(name, description, true)
    {
        protected override async Task<bool> ExecuteAsync(ConsoleOut console, CancellationToken token = default, params KeyValuePair<string, string>[] options)
        {
            var i = 0;

            while (i < 10)
            {
                await Task.Delay(1550, token);
                i++;
            }

            console.Write("This was a long running command");
            return true;
        }
    }
}
```

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/main/screenshots/3.png" />
</p>

### References ###
- Some commands might be long running tasks, Blazor.CommandLine use some features of a great component for this kind of requirement. Please also check  https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator if you need a loading indicator for a Blazor app.

