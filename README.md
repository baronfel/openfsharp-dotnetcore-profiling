# .Net Core Profiling examples

This repository contains the slides and code for my OpenF# 2019 presentation **An introduction to profiling and performance analysis in .NET Core 3**.  It consists of the slides at the top-level (in .ppt and .pdf variants) as individual folders for each of the three worked examples I cover in the talk.

## Prerequisites

Each example is a self-contained project with dockerfiles to build and run the sample, so the only real requirement should be having docker installed on your machine. Check the `README.md` in each folder for additional details.

## Topics

### Debugging memory leaks

This sample covers how to use the [dotnet-dump] tool to collect and analyze memory dumps of a running application.  It will also cover more in-depth investigations using LLDB directly.

### Profiling CPU usage

This sample covers how to use [dotnet-trace] and [speedscope] to collect and view CPU traces for your application. It also demonstrates the use of the [perf] tool on Linux for on-device profiling when a graphical user interface isn't available.

### Diagnosing logic errors

This sample covers how to use your debugger's thread information together with [dotnet-sos] to investigate incorrect logic in your application.


## Useful links

* the [dotnet/diagnostics] repository
* the .Net Core 3.0 SDK [download page](https://dotnet.microsoft.com/download/dotnet-core/3.0)

[dotnet/diagnostics]: https://github.com/dotnet/diagnostics/
[dotnet-dump]: https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-dump-instructions.md
[dotnet-trace]: https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-trace-instructions.md
[dotnet-counters]: https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-counters-instructions.md
[dotnet-sos]: https://github.com/dotnet/diagnostics/blob/master/documentation/installing-sos-instructions.md
[speedscope]: https://github.com/jlfwong/speedscope
[perf]: https://perf.wiki.kernel.org/index.php/Main_Page
