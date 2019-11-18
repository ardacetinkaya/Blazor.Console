# Blazor.Console

### What and why... ###

This is a simple console component for ASP.NET Core 3.0 Blazor Server application model. The motivation behind this simple component is to provide simple command-line interface to manage some Web API. With this component it is easy to execute some business related commands.* 

Real life scenario examples;
- Clear response cache
- Export log files
- Change run-time settings
- Monitor application resources


### Usage ###

Startup.cs

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

.razor file

```html
@page "/"
<h1>Hello, world!</h1>

<BlazorConsole />
```

Also add CSS reference to _HOST.cshtml

```html
<link href="Blazor.Console/styles.css" rel="stylesheet" />
```

<img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/1.png" >

<img src="https://github.com/ardacetinkaya/Blazor.Console/blob/master/screenshots/2.png" >

### References ###
- For long running commands, progress tracker: https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator 

