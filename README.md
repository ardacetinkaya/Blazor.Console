# Blazor.CommandLine (a.k.a Blazor.Console)

[![Actions Status](https://github.com/ardacetinkaya/Blazor.Console/workflows/Build/badge.svg)](https://github.com/ardacetinkaya/Blazor.Console/actions)

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/1.png" />
</p>

### What and why... ###

This is a simple console component for ASP.NET Core 3.0 Blazor Server application model. The motivation behind this simple component is to provide simple command-line interface to manage some Web API. With this component it is easy to execute some business related commands.* 

Real life scenario examples;
- Clear response cache
- Export log files
- Change run-time settings
- Monitor application resources

This Blazor component is based on ```System.CommandLine.Experimental``` API and reflects standart command-line features to be more reliable.

### Usage ###

Check Demo project(BlazorConsoleDemo.csproj) for usage;

Add following extensions to services and application within Startup.cs

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddCommandLine();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseCommandLine(env.WebRootPath);
}
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
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/2.png" />
</p>

### Commands ###

Adding commands to Blazor.CommandLine is not complicated. First create a command class, implement it with ```BaseCommand``` . Then add the command(s) to the BlazorCommandLine's Commands as below;

Within custom command's constructor it is easy to add options to the command. Also command arguments can be enabled for the command as ```System.CommandLine.Experimental```

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
    BlazorCommandLine console;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        console.Commands.Add(new CommandExample("simple","Description of command"));
        console.Commands.Add(new LongCommand("lng","Description of long-running command"));

        return base.OnAfterRenderAsync(firstRender);
    }

    public class CommandExample : BaseCommand
    {
        public CommandExample(string name,string description):base(name,description)
        {
            base.AddOption("-t","Description of option -t");
            base.AddOption("-ar","Description of option -ar");

            base.UseArguments($"Some extra arguments for {name}");
        
        }

        public override bool Execute(DefaultStreamWriter console,string option1,string option2,string option3,string option4,List<string> arguments)
        {
            console.Write("This is output of a simple command");
            return true;
        }
    }

    public class LongCommand : BaseCommand
    {
        public LongCommand(string name, string description):base(name,description,true)
        {
            
        }

        public override async Task<bool> ExecuteAsync(DefaultStreamWriter console, string option1,string option2,string option3,string optionArgument4,List<string> arguments)
        {
            var i = 0;

            while (i < 10)
            {
                await Task.Delay(550);
                i++;
            }
            
            console.Write("This was a long running command");
            return true;
        }
    }
}
```

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/3.png" />
</p>

### References ###
- Some commands might be long running tasks, Blazor.CommandLine use some features of a great component for this kind of requirement. Please also check  https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator if you need a loading indicator for a Blazor app.

