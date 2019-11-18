# Blazor.Console

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


### Usage ###

Check Demo project(BlazorConsoleDemo.csproj) for usage;

Add following extensions to services and application within Startup.cs

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddConsole();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseConsole(env.WebRootPath);
}
```

Add <BlazorConsole> tag into needed view file(ex: Index.razor)

```html
@page "/"
<h1>Hello, world!</h1>

<BlazorConsole />
```

And to have fancy UI add CSS to host file, _HOST.cshtml

```html
<link href="Blazor.Console/styles.css" rel="stylesheet" />
```

### Commands ###

Adding commands to Blazor.Console is not complicated. First create a command class, implement it with ```ICommand``` interface. Then add the command(s) to the BlazorConsole's Commands as below;

```cshtml
@page "/"
@using Blazor.Console.Command

<h1>Hello, world!</h1>

<BlazorConsole @ref="console" />

@code
{
    BlazorConsole console;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        console.Commands.Add("test", new CommandExample());
        
        return base.OnAfterRenderAsync(firstRender);
    }

    public class CommandExample : ICommand
    {
        public string Output { get; set; }
        public string Help { get; } = "Description of a Command Example";
        public async Task<string> Run(params string[] arguments)
        {
            Output = "Hello from a command";

            return Output;
        }
    }
}
```

### Long running Commands ###

Sometimes commands might have a long period. For these kind of commands inject ```IRunningCommand``` 

```cshtml
@page "/"
@using Blazor.Console.Command

@inject IRunningCommand RunningCommand


<h1>Hello, world!</h1>

<BlazorConsole @ref="console" />

@code
{
    BlazorConsole console;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        console.Commands.Add("lng", new LongCommand(RunningCommand));
        
        return base.OnAfterRenderAsync(firstRender);
    }

    public class LongCommand : ICommand
    {
        readonly IRunningCommand _loadingService;

        public string Output { get; set; }
        public string Help { get; };

        public LongCommand(IRunningCommand loadingService)
        {
            _loadingService = loadingService;
        }

        public async Task<string> Run(params string[] arguments)
        {
            await _loadingService.StartCommandAsync(async (task) =>
             {
                 task.Maintext = "Execution is started...";
                 var i = 0;

                 while (i < 10)
                 {
                     await Task.Delay(550);
                     task.Subtext = "Progress: " + i++;
                 }

                 task.Maintext = "Execution is completed.";
             });

            Output = "This was a long running command";

            return Output;
        }

    }
}

```
<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/3.gif" >
</p>

### Built-in Commands ###

```
help
os
version
```

Also added custom commands are displayed within help command

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/2.png" >
</p>

### References ###
- Some commands might be long running tasks, Blazor.Console use some features of a great component for this kind of requirement. Please also check  https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator if you need a loading indicator for a Blazor app.

