using System;

namespace Blazor.CommandLine.Command;

public interface ICommandStatus : IDisposable
{
    double? ProgressValue { get; set; }

    double? ProgressMax { get; set; }

    string Maintext { get; set; }

    string Subtext { get; set; }

    Exception Exception { get; set; }

    void DismissException();
}
