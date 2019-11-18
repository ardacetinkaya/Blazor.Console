# Blazor.Console

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

<img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/1.png" >

<img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/2.png" >

### References ###
- Some commands might be long running tasks, Blazor.Console use some features of a great component for this kind of requirement. Please also check  https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator if you need a loading indicator for a Blazor app.

