# Blazor.CommandLine (Blazor.Console)

[![Actions Status](https://github.com/ardacetinkaya/Blazor.Console/workflows/Build/badge.svg)](https://github.com/ardacetinkaya/Blazor.Console/actions)
[![NuGet version (Blzr.Components.CommandLine)](https://img.shields.io/nuget/v/Blzr.Components.CommandLine.svg)](https://www.nuget.org/packages/Blzr.Components.CommandLine/)

A simple, web-based command-line interface (CLI) component for ASP.NET Core Blazor applications.

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/main/screenshots/1.png" />
</p>

## Why this exists?

This is a simple Blazor component to be able to add and run some commands for an application.
The motivation behind this simple component is to manage applications and run maintenance tasks directly from the browser, without needing to SSH into a server or expose complex API endpoints with some additional fancy UI. It wraps the powerful `System.CommandLine` library, giving you a familiar CLI experience right in your Blazor app.

It's great for some simple operations:
- Clearing caches
- Triggering background jobs
- Viewing local application files or system status
- Changing runtime settings
- or many more


## Features

- **Web-based Terminal:** Familiar command-line look and feel.
- **Easy Integration:** Drop it into any Blazor page.
- **Custom Commands:** Create your own commands by inheriting from a base class.
- **Arguments & Options:** Supports standard CLI arguments and options (e.g., `--force`, `-v`).
- **Async Support:** Handles long-running tasks with loading indicators.
- **Built on System.CommandLine:** Uses the standard .NET command line parser.

## Installation

Install the package via NuGet:

```bash
dotnet add package Blzr.Components.CommandLine
```

## Setup

1.  **Register Services:**
    In your `Program.cs` (or `Startup.cs`), add the command line services:

    ```csharp
    builder.Services.AddCommandLine();
    ```

2.  **Configure Middleware:**
    Map the static assets (CSS/JS) in your request pipeline:

    ```csharp
    app.UseCommandLine(app.Environment.WebRootPath);
    ```

3.  **Add Styles:**
    Add the CSS reference to your `_Host.cshtml` (Blazor Server) or `index.html` (Blazor WebAssembly):

    ```html
    <link href="_content/Blzr.Components.CommandLine/styles.css" rel="stylesheet" />
    ```
    *(Note: Check the actual path in your project, it might be `Blazor.CommandLine/styles.css` depending on older versions, but `_content/...` is standard for libraries).*

## Usage

Add the component to any Razor page (e.g., `Index.razor`):

```razor
@page "/"
@using Blazor.CommandLine
@using Blazor.Components.CommandLine
@using Blazor.Components.CommandLine.Console

<BlazorCommandLine @ref="_console" Name="My App CLI" />

@code {
    private BlazorCommandLine _console;

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Register your custom commands here
            _console.Commands.Add(new MyCustomCommand());
        }
        return base.OnAfterRenderAsync(firstRender);
    }
}
```

## Creating Custom Commands

To create a command, inherit from `BaseCommand` and override the `Execute` or `ExecuteAsync` method.

```csharp
using Blazor.CommandLine.Command;
using Blazor.Components.CommandLine.Console;

public class MyCustomCommand : BaseCommand
{
    public MyCustomCommand() : base("my-cmd", "A sample custom command")
    {
        // Define options: becomes --target (or -o1)
        AddOption("target", "Specify the target to operate on");
    }

    protected override bool Execute(ConsoleOut console, params KeyValuePair<string, string>[] options)
    {
        // Retrieve option values
        var target = options.FirstOrDefault(o => o.Key == "target").Value;

        if (string.IsNullOrEmpty(target))
        {
            console.Write("Error: Target is required.");
            return false;
        }

        console.Write($"Executing command on target: {target}");
        return true;
    }
}
```

### Async Commands

For long-running operations, use `ExecuteAsync`. The UI will show a loading indicator.

```csharp
public class LongRunningCommand : BaseCommand
{
    public LongRunningCommand() : base("process", "Starts a long process", longRunning: true)
    {
    }

    protected override async Task<bool> ExecuteAsync(ConsoleOut console, CancellationToken ct, params KeyValuePair<string, string>[] options)
    {
        console.Write("Starting process...");
        await Task.Delay(3000, ct); // Simulate work
        console.Write("Process completed successfully!");
        return true;
    }
}
```

<p align="center">
    <img src="https://github.com/ardacetinkaya/Blazor.Console/blob/main/screenshots/2.png" />
</p>

### References ###
- Some commands might be long running tasks, Blazor.CommandLine use some features of a great component for this kind of requirement. Please also check  https://github.com/h3x4d3c1m4l/Blazor.LoadingIndicator if you need a loading indicator for a Blazor app.
- `System.CommandLine` is evolved a lot and now it is much structured and solid way to use in applications. Please also check https://learn.microsoft.com/en-us/dotnet/standard/commandline/



## Contributing

This project was built to solve a specific need, but I'm sure there are many ways it can be improved. If you have ideas for new features, better styling, or bug fixes, I'd really appreciate your help!

Feel free to open an issue to discuss ideas or submit a pull request. Let's make this a much useful tool.

## License

This project is licensed under the MIT License.

